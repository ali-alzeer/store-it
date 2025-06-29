"use client";

import React, { useCallback, useContext, useState } from "react";

import { useDropzone } from "react-dropzone";
import { Button } from "../components/ui/button";
import {
  calculateTotalFileSize,
  cn,
  convertFileToUrl,
  getFileType,
} from "../lib/utils";
import Image from "next/image";
import Thumbnail from "../components/Thumbnail";
import {
  MAX_CAPACITY_SIZE_FOR_USER,
  MAX_FILE_SIZE,
  MAX_FILES_COUNT,
} from "../constants";
import { useToast } from "../hooks/use-toast";
import { getAllFilesForUser, uploadFile } from "../lib/actions/file.actions";
import { FilesContext } from "../contexts/FilesContext";

interface Props {
  user: User | null;
  className?: string;
}

const FileUploader = ({ user, className }: Props) => {
  const { toast } = useToast();
  const [files, setFiles] = useState<File[]>([]);
  const [, setAllFiles] = useState<UserFileDto[]>([]);

  const { notification, setNotification } = useContext(FilesContext);

  const onDrop = useCallback(async (acceptedFiles: File[]) => {
    setFiles(acceptedFiles);

    const uploadPromises = acceptedFiles.map(async (file) => {
      if (!user) {
        setAllFiles([]);
        setFiles([]);
        if (window.innerWidth <= 640) {
          window.alert(`You are not authorized`);
        }
        return toast({
          description: (
            <p className="body-2 text-white">You are not authorized</p>
          ),
          className: "error-toast",
        });
      }

      const filesErrorObject = await getAllFilesForUser(user);

      if (filesErrorObject.files === null) {
        setAllFiles([]);
        setFiles([]);
        if (window.innerWidth <= 640) {
          window.alert(`An error occurred, please try again later`);
        }
        return toast({
          description: (
            <p className="body-2 text-white">
              An error occurred, please try again later
            </p>
          ),
          className: "error-toast",
        });
      }

      setAllFiles(filesErrorObject.files);

      if (filesErrorObject.files.length > MAX_FILES_COUNT - 1) {
        setFiles((prevFiles) => prevFiles.filter((f) => f.name !== file.name));
        if (window.innerWidth <= 640) {
          window.alert(
            `You have reached the maximum number of files (100 files for user)`
          );
        }
        return toast({
          description: (
            <p className="body-2 text-white">
              You have reached the maximum number of files {"("}100 files for
              user{")"}
            </p>
          ),
          className: "error-toast",
        });
      }

      const totalFilesSizes =
        calculateTotalFileSize(filesErrorObject.files) + file.size;

      if (totalFilesSizes >= MAX_CAPACITY_SIZE_FOR_USER) {
        setFiles((prevFiles) => prevFiles.filter((f) => f.name !== file.name));
        if (window.innerWidth <= 640) {
          window.alert(`You do not have enough space to upload`);
        }
        return toast({
          description: (
            <p className="body-2 text-white">
              You do not have enough space to upload
            </p>
          ),
          className: "error-toast",
        });
      }

      if (file.size > MAX_FILE_SIZE) {
        setFiles((prevFiles) => prevFiles.filter((f) => f.name !== file.name));
        if (window.innerWidth <= 640) {
          window.alert(`${file.name} is too large.
              Max file size is 5MB`);
        }
        return toast({
          description: (
            <p className="body-2 text-white">
              <span className="font-semibold">{file.name}</span> is too large.
              Max file size is 5MB
            </p>
          ),
          className: "error-toast",
        });
      }

      if (user !== null && user !== undefined) {
        const fileObject = await uploadFile({ file, user });

        if (fileObject.file === null) {
          setFiles((prevFiles) =>
            prevFiles.filter((f) => f.name !== file.name)
          );
          if (window.innerWidth <= 640) {
            window.alert(`${file.name} failed to
                upload : ${fileObject.error}`);
          }
          return toast({
            description: (
              <p className="body-2 text-white">
                <span className="font-semibold">{file.name}</span> failed to
                upload : {fileObject.error}
              </p>
            ),
            className: "error-toast",
          });
        } else {
          setFiles((prevFiles) =>
            prevFiles.filter((f) => f.name !== file.name)
          );
          if (window.innerWidth <= 640) {
            window.alert(`${file.name} uploaded
                successfully`);
          }
          return toast({
            description: (
              <p className="body-2 text-white">
                <span className="font-semibold">{file.name}</span> uploaded
                successfully
              </p>
            ),
            className: "success-toast",
          });
        }
      }
    });

    await Promise.all(uploadPromises);
    setNotification(!notification);
  }, []);

  const { getRootProps, getInputProps } = useDropzone({ onDrop });

  const handleRemoveFile = (
    e: React.MouseEvent<HTMLImageElement, MouseEvent>,
    fileName: string
  ) => {
    e.stopPropagation();
    setFiles((prevFiles) => prevFiles.filter((file) => file.name !== fileName));
  };

  return (
    <div {...getRootProps()} className="cursor-pointer">
      <input {...getInputProps()} />
      <Button
        type="button"
        className={cn(
          "uploader-button",
          "text-start",
          "flex",
          "justify-start",
          "w-full",
          className
        )}
      >
        <Image
          src="/assets/icons/upload.svg"
          alt="upload"
          width={24}
          height={24}
        />{" "}
        <p>Upload</p>
      </Button>
      {files.length > 0 && (
        <ul className="uploader-preview-list">
          <h4 className="h4 text-light-100">Uploading</h4>

          {files.map((file, index) => {
            const { type, extension } = getFileType(file.name);

            return (
              <li
                key={`${file.name}-${index}`}
                className="uploader-preview-item"
              >
                <div className="flex items-center gap-3">
                  <Thumbnail
                    type={type}
                    extension={extension}
                    url={convertFileToUrl(file)}
                  />

                  <div className="preview-item-name">
                    {file.name}
                    <Image
                      src="/assets/icons/file-loader.gif"
                      width={80}
                      height={26}
                      alt="Loader"
                      className="mt-2"
                    />
                  </div>
                </div>

                <Image
                  src="/assets/icons/remove.svg"
                  width={24}
                  height={24}
                  alt="Remove"
                  onClick={(e) => handleRemoveFile(e, file.name)}
                />
              </li>
            );
          })}
        </ul>
      )}
    </div>
  );
};

export default FileUploader;
