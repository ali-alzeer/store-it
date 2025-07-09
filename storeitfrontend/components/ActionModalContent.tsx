"use client";

import React, { useEffect } from "react";
import Thumbnail from "./Thumbnail";
import { convertFileSize, formatDateTime } from "../lib/utils";
import { Input } from "./ui/input";
import Image from "next/image";
import { User2Icon, XIcon } from "lucide-react";
import { getAllUsersForFile } from "../lib/actions/file.actions";

const ImageThumbnail = ({ file }: { file: UserFileDto }) => {
  return (
    <div className="file-details-thumbnail">
      <Thumbnail
        type={file.fileTypeName}
        extension={file.fileExtensionName}
        url={file.url}
      />
      <div className="text-start flex flex-col">
        <p className="subtitle-2 mb-1">{file.fileName}</p>
        <p className="caption">{formatDateTime(file.createdAt.toString())}</p>
      </div>
    </div>
  );
};

const DetailRow = ({
  label,
  value,
  capitalize,
}: {
  label: string;
  value: string;
  capitalize: boolean;
}) => {
  return (
    <div className="flex">
      <p className="file-details-label text-left">{label}</p>
      {capitalize ? (
        <p className="file-details-value text-left capitalize">{value}</p>
      ) : (
        <p className="file-details-value text-left">{value}</p>
      )}
    </div>
  );
};

export const FileDetails = ({ file }: { file: UserFileDto }) => {
  return (
    <>
      <ImageThumbnail file={file} />
      <div className="space-y-4 px-2 pt-2">
        <DetailRow
          capitalize={false}
          label="Format: "
          value={file.fileExtensionName}
        />
        <DetailRow
          capitalize={false}
          label="Size: "
          value={convertFileSize(file.size)}
        />
        <DetailRow
          capitalize={true}
          label="Owner: "
          value={`${file.ownerFirstName} ${file.ownerLastName}`}
        />
      </div>
    </>
  );
};

interface Props {
  file: UserFileDto;
  onInputChange: React.Dispatch<React.SetStateAction<string[]>>;
  onRemove: (userEmailToRemove: string) => void;
  usersForFile: UserFileShared[];
  setUsersForFile: React.Dispatch<React.SetStateAction<UserFileShared[]>>;
  isLoading: boolean;
  setIsLoading: React.Dispatch<React.SetStateAction<boolean>>;
  user: User | null;
  error: string;
  notifyChild: boolean;
  setError: React.Dispatch<React.SetStateAction<string>>;
}

export const ShareInput = ({
  file,
  onInputChange,
  onRemove,
  usersForFile,
  setUsersForFile,
  isLoading,
  setIsLoading,
  user,
  notifyChild,
  setError,
}: Props) => {
  useEffect(() => {
    if (!user) {
      return;
    }

    const getUsersForFile = async () => {
      setIsLoading(true);
      let usersErrorObject: // eslint-disable-next-line @typescript-eslint/no-explicit-any
      { users: any; error: null } | { users: null; error: string } = {
        users: [],
        error: null,
      };
      try {
        usersErrorObject = await getAllUsersForFile(user, file.fileId);
        setUsersForFile(usersErrorObject.users ?? []);
        setError("");
      } catch (error) {
        console.log("Error occurred : ", error);
        setUsersForFile([]);
        if (usersErrorObject.error !== null) {
          setError(usersErrorObject.error);
        } else {
          setError("Unknown error occurred");
        }
      } finally {
        setIsLoading(false);
      }
    };

    getUsersForFile();

    return () => {};
  }, [notifyChild]);

  return (
    <>
      {isLoading ? (
        <div className="bg-gray-200 p-5 rounded-4xl h-full overflow-hidden">
          <div className="w-full h-full flex flex-col justify-center items-center gap-3 bg-gray-200">
            <div className="w-10 h-10 border-4 border-transparent border-t-brand animate-spin rounded-full"></div>
          </div>
        </div>
      ) : (
        <>
          <ImageThumbnail file={file} />
          <div className="share-wrapper">
            <p className="subtitle-2 pl-1 text-light-100">
              Share file with other users
            </p>
            <Input
              data-testid="shareInput"
              type="email"
              placeholder="ex1@ex1.com , ex2@ex2.com ..."
              onChange={(e) => onInputChange(e.target.value.trim().split(","))}
              className="share-input-field"
            />
            <div className="pt-4">
              <div className="flex flex-col justify-between">
                <p className="subtitle-2 text-light-100 mb-2">Shared with</p>

                {usersForFile.length > 1 ? (
                  <div className="gap-2 subtitle-2 text-light-200 flex flex-col justify-center items-center text-wrap">
                    {usersForFile.map((u) => {
                      if (u.id !== file.ownerId) {
                        return (
                          <div
                            key={u.id}
                            className="text-start w-full rounded-full p-2 bg-gray-200 text-black flex items-center gap-3"
                          >
                            {u.imageUrl ? (
                              <Image
                                src={u.imageUrl!}
                                alt="Avatar"
                                width={40}
                                height={40}
                                className="rounded-full object-cover h-10 w-10"
                              />
                            ) : (
                              <User2Icon className="rounded-full h-10 w-10 bg-gray-100 p-1" />
                            )}
                            <div className="flex-1 flex flex-col">
                              <p className="text-xs md:text-sm capitalize text-wrap">
                                {u.firstName} {u.lastName}
                              </p>
                              <p className="text-xs md:text-sm text-wrap">
                                {u.email}
                              </p>
                            </div>
                            {u.id === user?.id || file.ownerId == user?.id ? (
                              <XIcon
                                className="cursor-pointer rounded-full h-10 w-10 text-white bg-gray-600 p-1"
                                onClick={() => onRemove(u.email)}
                              />
                            ) : null}
                          </div>
                        );
                      }
                    })}
                  </div>
                ) : (
                  <p>No users share this file</p>
                )}
              </div>
            </div>
          </div>
        </>
      )}
    </>
  );
};
