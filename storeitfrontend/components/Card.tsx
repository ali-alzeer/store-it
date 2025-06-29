import React, { useContext } from "react";
import { convertFileSize, formatDateTime } from "../lib/utils";
import Thumbnail from "./Thumbnail";
import ActionDropDown from "./ActionDropDown";
import { User2Icon } from "lucide-react";
import Image from "next/image";
import { AuthContext } from "../contexts/AuthContext";

const Card = ({ file }: { file: UserFileDto }) => {
  const { user } = useContext(AuthContext);

  return (
    <div
      className={
        user?.id === file.ownerId
          ? "file-card bg-white"
          : "file-card shared-card"
      }
    >
      <div className="flex justify-between">
        <Thumbnail
          type={file.fileTypeName}
          extension={file.fileExtensionName}
          className="size-20"
          imageClassName="size-11"
          url={file.url}
        />
        <div className="flex flex-col items-end justify-between gap-1">
          <ActionDropDown file={file} />
          <p className="text-xs text-nowrap line-clamp-1 text-center sm:text-sm sm:text-start md:text-base">
            {convertFileSize(file.size)}
          </p>
        </div>
      </div>

      <div className="file-card-details">
        <p className="subtitle-2 line-clamp-1">{file.fileName}</p>
        <p className="text-xs text-gray-500">
          {formatDateTime(file.createdAt.toString())}
        </p>
        <div
          className={
            file.ownerId === user?.id
              ? "overflow-hidden border-[1px] border-gray-600 rounded-full bg-gray-200 p-2 text-gray-600 flex items-center gap-1.5"
              : "overflow-hidden border-[1px] border-gray-600 rounded-full bg-brand p-2 text-white flex items-center gap-1.5"
          }
        >
          {file.ownerImageUrl ? (
            <Image
              src={file.ownerImageUrl!}
              alt="Avatar"
              width={20}
              height={20}
              className="rounded-full object-cover h-5 w-5"
            />
          ) : (
            <User2Icon className="rounded-full h-5 w-5 border-[1px]" />
          )}
          <p className="text-xs text-nowrap md:text-sm capitalize overflow-hidden line-clamp-1">
            {file.ownerFirstName} {file.ownerLastName}
          </p>
        </div>
      </div>
    </div>
  );
};

export default Card;
