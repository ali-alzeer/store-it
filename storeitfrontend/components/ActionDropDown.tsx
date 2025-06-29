"use client";

import {
  Dialog,
  DialogContent,
  DialogFooter,
  DialogHeader,
  DialogTitle,
} from "./ui/dialog";
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuLabel,
  DropdownMenuSeparator,
  DropdownMenuTrigger,
} from "./ui/dropdown-menu";
import React, { useContext, useState } from "react";
import { actionsDropdownItems } from "../constants";
import Image from "next/image";
import { Input } from "./ui/input";
import { Button } from "./ui/button";
import {
  deleteFile,
  removeUserForSharedFile,
  renameFile,
  shareFile,
} from "../lib/actions/file.actions";
import { AuthContext } from "../contexts/AuthContext";
import { FilesContext } from "../contexts/FilesContext";
import { useToast } from "../hooks/use-toast";
import { FileDetails, ShareInput } from "./ActionModalContent";

const ActionDropDown = ({ file }: { file: UserFileDto }) => {
  const [isModalOpen, setIsModelOpen] = useState(false);
  const [isDropdownOpen, setIsDropdownOpen] = useState(false);
  const [action, setAction] = useState<ActionType | null>(null);
  const [name, setName] = useState(file.fileName);
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState("");
  const [usersForFile, setUsersForFile] = useState<UserFileShared[]>([]);
  const [emails, setEmails] = useState<string[]>([]);
  const [notifyChild, setNotifyChild] = useState(false);

  const { notification, setNotification } = useContext(FilesContext);

  const { user } = useContext(AuthContext);
  const { toast } = useToast();

  const closeAllModals = () => {
    setIsModelOpen(false);
    setIsDropdownOpen(false);
    setAction(null);
    setName(file.fileName);
    setIsLoading(false);
    setError("");
  };

  const handleAction = async () => {
    console.log(action);

    if (!action) return;
    if (!user) return;
    if (action.value === "share") {
      if (emails.length === 0) return;
    }
    if (action.value === "rename") {
      if (name.length === 0) return;
    }

    setIsLoading(true);

    const actions = {
      rename: () => {
        const fileToRename: FileRenameDto = {
          fileId: file.fileId,
          fileName: name,
          user: user,
        };

        if (name.split(".").pop() !== file.fileExtensionName) {
          fileToRename.fileName = `${name}.${file.fileExtensionName}`;
        }

        return renameFile(fileToRename);
      },
      share: () => {
        const fileToShare: FileShareDto = {
          emails: emails,
          fileId: file.fileId,
          user: user,
        };

        return shareFile(fileToShare);
      },
      delete: () => {
        const fileToDelete: FileDeletionDto = {
          fileId: file.fileId,
          fileUrl: file.url,
          user: user,
        };
        return deleteFile(fileToDelete);
      },
    };

    const result = await actions[action.value as keyof typeof actions]();
    if (result.error !== null) {
      setIsLoading(false);
      setError(result.error);
      return;
    }

    if (result.success !== null) {
      if (action.value === "share") {
        setNotifyChild(!notifyChild);
        setIsLoading(false);
        return;
      } else {
        closeAllModals();
        setNotification(!notification);
        return;
      }
    } else {
      setIsLoading(false);
      setError("Unknown error occurred");
      return;
    }
  };

  const handleDownload = async (file: UserFileDto) => {
    try {
      const response = await fetch(file.url);
      if (!response.ok) {
        if (window.innerWidth <= 640) {
          window.alert(`Failed to download ${file.fileName}`);
        }
        return toast({
          description: (
            <p className="body-2 text-white">
              Failed to download {file.fileName}
            </p>
          ),
          className: "error-toast",
        });
      }
      const blob = await response.blob();
      const url = window.URL.createObjectURL(blob);
      const a = document.createElement("a");
      a.style.display = "none";
      a.href = url;
      a.setAttribute("download", file.fileName); // specify the file name
      document.body.appendChild(a);
      a.click();
      window.URL.revokeObjectURL(url);
    } catch (error) {
      console.error(error);
    }
  };

  const handleRemoveUser = async (emailToRemove: string) => {
    if (!user) return;

    const fileRemoveShareDto: FileRemoveShareDto = {
      fileId: file.fileId,
      userEmailToRemove: emailToRemove,
      user: user,
    };
    setIsLoading(true);

    const result = await removeUserForSharedFile(fileRemoveShareDto);

    if (result.error !== null) {
      setIsLoading(false);
      setError(result.error);
      return;
    }

    if (result.success !== null) {
      if (file.ownerId === user.id) {
        setNotifyChild(!notifyChild);
        setIsLoading(false);
        return;
      } else {
        closeAllModals();
        setNotification(!notification);
        return;
      }
    } else {
      setIsLoading(false);
      setError("Unknown error occurred");
      return;
    }
  };

  const renderDialogContent = () => {
    if (!action) return null;

    const { value, label } = action;

    return (
      <DialogContent className="shad-dialog button">
        <DialogHeader className="flex flex-col gap-3">
          <DialogTitle className="text-center text-light-100">
            {label}
          </DialogTitle>
          {value === "rename" && (
            <Input
              type="text"
              value={name}
              onChange={(e) => setName(e.target.value)}
            />
          )}
          {value === "delete" && (
            <p className="text-center w-full">
              Are you sure about deleting{" "}
              <span className="font-bold">{name}</span>
              {" ?"}
            </p>
          )}
          {value === "details" && <FileDetails file={file} />}
          {value === "share" && (
            <ShareInput
              file={file}
              usersForFile={usersForFile}
              setUsersForFile={setUsersForFile}
              onInputChange={setEmails}
              onRemove={handleRemoveUser}
              isLoading={isLoading}
              setIsLoading={setIsLoading}
              user={user}
              notifyChild={notifyChild}
              error={error}
              setError={setError}
            />
          )}
        </DialogHeader>
        {error !== "" ? (
          <div className="text-red text-center w-full text-xs sm:text-sm">
            {error}
          </div>
        ) : null}
        {["rename", "delete", "share"].includes(value) && (
          <DialogFooter className="flex flex-col gap-3 md:flex-row">
            <Button onClick={closeAllModals} className="modal-cancel-button">
              Cancel
            </Button>
            <Button
              disabled={isLoading}
              onClick={handleAction}
              className="modal-submit-button"
            >
              <p className="capitalize">{value}</p>
              {isLoading && (
                <Image
                  src="/assets/icons/loader.svg"
                  alt="loader"
                  width={24}
                  height={24}
                  className="animate-spin"
                />
              )}
            </Button>
          </DialogFooter>
        )}
      </DialogContent>
    );
  };

  return (
    <Dialog open={isModalOpen} onOpenChange={setIsModelOpen}>
      <DropdownMenu open={isDropdownOpen} onOpenChange={setIsDropdownOpen}>
        <DropdownMenuTrigger className="shad-no-focus">
          <div className="relative cursor-pointer p-5 flex flex-col justify-center items-center gap-1">
            <div className="absolute top-1 right-1 flex flex-col justify-center items-center gap-1">
              <div className="w-1 h-1 border-[1px] border-gray-500 rounded-full"></div>
              <div className="w-1 h-1 border-[1px] border-gray-500 rounded-full"></div>
              <div className="w-1 h-1 border-[1px] border-gray-500 rounded-full"></div>
            </div>
          </div>
        </DropdownMenuTrigger>
        <DropdownMenuContent className="max-w-10">
          <DropdownMenuLabel className="line-clamp-1">
            {file.fileName}
          </DropdownMenuLabel>
          <DropdownMenuSeparator />
          {actionsDropdownItems.map((actionItem) => {
            if (actionItem.value === "delete" && file.ownerId !== user?.id) {
              return null;
            }

            return (
              <DropdownMenuItem
                key={actionItem.value}
                className="shad-dropdown-item"
                onClick={() => {
                  setAction(actionItem);
                  if (
                    ["rename", "share", "delete", "details"].includes(
                      actionItem.value
                    )
                  ) {
                    setIsModelOpen(true);
                  }
                }}
              >
                {actionItem.value === "download" ? (
                  <div
                    onClick={() => handleDownload(file)}
                    className="flex items-center gap-2"
                  >
                    <Image
                      width={30}
                      height={30}
                      src={actionItem.icon}
                      alt={actionItem.label}
                    />
                    {actionItem.label}
                  </div>
                ) : (
                  <div className="flex items-center gap-2">
                    <Image
                      width={30}
                      height={30}
                      src={actionItem.icon}
                      alt={actionItem.label}
                    />
                    {actionItem.label}
                  </div>
                )}
              </DropdownMenuItem>
            );
          })}
        </DropdownMenuContent>
      </DropdownMenu>
      {renderDialogContent()}
    </Dialog>
  );
};

export default ActionDropDown;
