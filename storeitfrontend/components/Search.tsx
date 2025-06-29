"use client";

import Image from "next/image";
import React, { useContext, useState } from "react";
import { Input } from "./ui/input";
import { FilesContext } from "../contexts/FilesContext";
import Thumbnail from "./Thumbnail";
import { getFileType } from "../lib/utils";
import { redirect } from "next/navigation";

const Search = () => {
  const [query, setQuery] = useState("");
  const files = useContext(FilesContext);

  function GoToFilePage(file: UserFileDto) {
    const type = getFileType(file.fileName);
    if (type.type === "video" || type.type === "audio") {
      redirect(`media`);
    } else {
      redirect(`${type.type}s`);
    }
  }

  return (
    <div className="search relative">
      <div className="search-input-wrapper">
        <Image
          src="assets/icons/search.svg"
          alt="Search"
          width={24}
          height={24}
        />
        <Input
          value={query}
          placeholder="Search..."
          onChange={(e) => setQuery(e.target.value)}
        />
      </div>
      {query !== "" && files && files.files && files.files?.length > 0 ? (
        files.files.filter((f) => f.fileName.includes(query)).length > 0 ? (
          <div className="text-sm rounded-xl z-20 absolute shadow-2 p-3 overflow-hidden w-full flex flex-col gap-4 bg-white">
            {files.files
              .filter((f) => f.fileName.includes(query))
              .map((f) => {
                return (
                  <div
                    onClick={() => GoToFilePage(f)}
                    className="cursor-pointer p-2 rounded-lg hover:bg-gray-200 w-full flex gap-3 items-center"
                    key={f.fileId}
                  >
                    <Thumbnail
                      extension={f.fileExtensionName}
                      type={f.fileTypeName}
                      url={f.url}
                    />
                    <p className="line-clamp-1">{f.fileName}</p>
                  </div>
                );
              })}
          </div>
        ) : (
          <div className="text-center text-sm text-gray-600 rounded-xl z-20 absolute shadow-2 p-3 overflow-hidden w-full flex flex-col justify-center gap-4 bg-white">
            No results found
          </div>
        )
      ) : null}
    </div>
  );
};
export default Search;
