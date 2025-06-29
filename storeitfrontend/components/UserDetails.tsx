"use client";

import {
  Dialog,
  DialogContent,
  DialogFooter,
  DialogHeader,
  DialogTitle,
} from "./ui/dialog";
import { DropdownMenu, DropdownMenuTrigger } from "./ui/dropdown-menu";
import React, { ReactNode, useContext, useRef, useState } from "react";
import { MAX_FILE_SIZE } from "../constants";
import Image from "next/image";
import { Input } from "./ui/input";
import { Button } from "./ui/button";
import { AuthContext } from "../contexts/AuthContext";
import { FilesContext } from "../contexts/FilesContext";
import { DeleteIcon, PenIcon, User2Icon, XIcon } from "lucide-react";
import {
  changeUserImage,
  changeUserName,
  deleteUserAccount,
  deleteUserImage,
} from "../lib/actions/user.actions";
import { saveLoggedInUserToLocalStorage } from "../lib/utils";
import { redirect } from "next/navigation";

const UserDetails = ({
  renderUserDetails,
}: {
  renderUserDetails: () => ReactNode;
}) => {
  const [isModalOpen, setIsModelOpen] = useState(false);
  const [, setIsDropdownOpen] = useState(false);
  const [isEditName, setIsEditName] = useState(false);
  const [isDeleteAccount, setIsDeleteAccount] = useState(false);
  const [newFirstName, setNewFirstName] = useState<string>("");
  const [newLastName, setNewLastName] = useState<string>("");
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState("");

  const { notification, setNotification } = useContext(FilesContext);

  const { user, setUser } = useContext(AuthContext);

  const imageInputRef = useRef<HTMLInputElement>(null);

  const closeAllModals = () => {
    setIsModelOpen(false);
    setIsDropdownOpen(false);
    setIsEditName(false);
    setNewFirstName("");
    setNewLastName("");
    setIsLoading(false);
    setError("");
  };

  const handleChangeImage = async (
    event: React.ChangeEvent<HTMLInputElement>
  ) => {
    if (!user) return;

    const imageFile = event.target.files?.[0];

    if (!imageFile) {
      return;
    } else {
      if (imageFile.size > MAX_FILE_SIZE) return;

      const imageChangeDto: ImageChangeDto = {
        image: imageFile,
        user: user,
      };
      setIsLoading(true);

      const result = await changeUserImage(imageChangeDto);

      if (result.error !== null) {
        setIsLoading(false);
        setError(result.error);
        return;
      }

      if (result.user !== null) {
        setError("");
        saveLoggedInUserToLocalStorage(result.user);
        setUser(result.user);
        setNotification(!notification);
        setIsLoading(false);
        return;
      } else {
        setIsLoading(false);
        setError("Unknown error occurred");
        return;
      }
    }
  };

  const handleDeleteImage = async () => {
    if (!user) return;

    if (!user.imageUrl) return;

    const imageDeleteDto: ImageDeleteDto = {
      imageUrl: user.imageUrl,
      user: user,
    };
    setIsLoading(true);

    const result = await deleteUserImage(imageDeleteDto);

    if (result.error !== null) {
      setIsLoading(false);
      setError(result.error);
      return;
    }

    if (result.user !== null) {
      setError("");
      saveLoggedInUserToLocalStorage(result.user);
      setUser(result.user);
      setNotification(!notification);
      setIsLoading(false);
      return;
    } else {
      setIsLoading(false);
      setError("Unknown error occurred");
      return;
    }
  };

  const handleChangeName = async () => {
    if (!user) return;
    if (newFirstName === "" || newLastName === "") return;

    const nameChangeDto: NameChangeDto = {
      firstName: newFirstName,
      lastName: newLastName,
      user: user,
    };
    setIsLoading(true);

    const result = await changeUserName(nameChangeDto);

    if (result.error !== null) {
      setIsLoading(false);
      setError(result.error);
      return;
    }

    if (result.user !== null) {
      setError("");
      saveLoggedInUserToLocalStorage(result.user);
      setUser(result.user);
      setNotification(!notification);
      setIsLoading(false);
      return;
    } else {
      setIsLoading(false);
      setError("Unknown error occurred");
      return;
    }
  };

  const handleDeleteAccount = async () => {
    if (!user) return;

    setIsLoading(true);

    const result = await deleteUserAccount(user);

    if (result.error !== null) {
      setIsLoading(false);
      setError(result.error);
      return;
    }

    if (result.success !== null) {
      setError("");
      saveLoggedInUserToLocalStorage(null);
      setUser(null);
      setNotification(!notification);
      setIsLoading(false);
      redirect("/");
    } else {
      setIsLoading(false);
      setError("Unknown error occurred");
      return;
    }
  };

  const renderDialogContent = () => {
    if (!user) return;

    return (
      <DialogContent className="shad-dialog button">
        <DialogHeader className="flex flex-col gap-3">
          <DialogTitle className="text-center text-light-100">
            User Details
          </DialogTitle>
        </DialogHeader>

        {isLoading ? (
          <div className="p-5 rounded-4xl h-full overflow-hidden">
            <div className="w-full h-full flex flex-col justify-center items-center gap-3">
              <div className="w-20 h-20 border-4 border-transparent border-t-brand animate-spin rounded-full"></div>
            </div>
          </div>
        ) : (
          <div className=" w-full flex flex-col justify-center items-center text-center gap-5 py-3">
            <div className="relative rounded-full bg-gray-300 w-36 h-36 flex justify-center items-center">
              {user.imageUrl ? (
                <Image
                  src={user.imageUrl!}
                  alt="Avatar"
                  width={144}
                  height={144}
                  className="w-full h-full object-cover rounded-full"
                />
              ) : (
                <User2Icon className="w-20 h-20" />
              )}
              <div
                onClick={() => imageInputRef.current?.click()}
                className="cursor-pointer absolute bottom-0 right-0 rounded-full bg-brand w-10 h-10 flex justify-center items-center"
              >
                <Input
                  type="file"
                  className="hidden"
                  accept=".jpg,.jpeg,.png,.gif,.bmp,.svg,.webp"
                  onChange={handleChangeImage}
                  ref={imageInputRef}
                />
                <PenIcon className="w-5 h-5 text-white" />
              </div>
              {user.imageUrl ? (
                <div
                  onClick={handleDeleteImage}
                  className="cursor-pointer absolute bottom-0 left-0 rounded-full bg-error w-10 h-10 flex justify-center items-center"
                >
                  <XIcon className="w-5 h-5 text-white" />
                </div>
              ) : null}
            </div>
            {error !== "" ? (
              <div className="text-red text-center w-full text-xs sm:text-sm">
                {error}
              </div>
            ) : null}
            <div className="overflow-hidden w-full border-2 border-gray-400 rounded-lg">
              <div className="max-w-full p-5">
                <p className="font-bold text-base line-clamp-1 capitalize w-full">
                  {user.firstName} {user.lastName}
                </p>
                <p className="caption">{user.email}</p>
              </div>

              <div
                onClick={() => setIsEditName(!isEditName)}
                className="cursor-pointer bg-brand text-white p-3 flex justify-center items-center"
              >
                {isEditName ? (
                  <>
                    <p className="mr-1">Cancel changing name </p>{" "}
                    <XIcon></XIcon>
                  </>
                ) : (
                  <>
                    <p className="mr-1">Change name </p> <PenIcon></PenIcon>
                  </>
                )}
              </div>
              {isEditName ? (
                <div className="bg-brand-25 p-3 flex justify-center items-center flex-col gap-2">
                  <Input
                    type="text"
                    placeholder="First name"
                    value={newFirstName}
                    onChange={(e) => setNewFirstName(e.target.value)}
                    className="border-[1px] border-gray-500"
                    autoFocus
                  />
                  <Input
                    type="text"
                    placeholder="Last name"
                    value={newLastName}
                    onChange={(e) => setNewLastName(e.target.value)}
                    className="border-[1px] border-gray-500"
                  />
                  <DialogFooter className=" flex flex-col gap-3 md:flex-row w-full">
                    <Button
                      onClick={closeAllModals}
                      className="modal-cancel-button"
                    >
                      Cancel
                    </Button>
                    <Button
                      disabled={isLoading}
                      className="modal-submit-button"
                      onClick={handleChangeName}
                    >
                      <p className="capitalize">Save</p>
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
                </div>
              ) : null}

              <div
                onClick={() => setIsDeleteAccount(!isDeleteAccount)}
                className="cursor-pointer bg-brand text-white p-3 flex justify-center items-center"
              >
                {isDeleteAccount ? (
                  <>
                    <p className="mr-1">Cancel deleting account </p>{" "}
                    <XIcon></XIcon>
                  </>
                ) : (
                  <>
                    <p className="mr-1">Delete account </p>{" "}
                    <DeleteIcon></DeleteIcon>{" "}
                  </>
                )}
              </div>

              {isDeleteAccount ? (
                <div className="bg-brand-25 p-3 flex justify-center items-center flex-col gap-2">
                  <div className="flex flex-col gap-3 w-full text-xs">
                    <p className="text-error">
                      If you delete your account all your files will be deleted
                      and you can not get them back
                    </p>
                    <p className="text-brand">
                      it will take some time to delete your account so do not
                      close the window until deleting is finished
                    </p>
                    <p>Are you sure about deleting your account?</p>
                    <Button
                      onClick={closeAllModals}
                      className="modal-cancel-button"
                    >
                      Cancel
                    </Button>
                    <Button
                      disabled={isLoading}
                      className="modal-submit-button bg-error"
                      onClick={handleDeleteAccount}
                    >
                      <p className="capitalize">Ok</p>
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
                  </div>
                </div>
              ) : null}
            </div>
          </div>
        )}
      </DialogContent>
    );
  };

  if (!user) return;

  return (
    <Dialog open={isModalOpen} onOpenChange={setIsModelOpen}>
      <>
        <DropdownMenu open={true} onOpenChange={setIsDropdownOpen}>
          <DropdownMenuTrigger
            className="shad-no-focus"
            onClick={() => setIsModelOpen(true)}
          >
            {renderUserDetails()}
          </DropdownMenuTrigger>
        </DropdownMenu>
        {renderDialogContent()}
      </>
    </Dialog>
  );
};

export default UserDetails;
