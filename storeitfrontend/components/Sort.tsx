"use client";

import { ArrowDownIcon, ArrowUpIcon } from "lucide-react";
import React, { useEffect, useState } from "react";

const Sort = ({
  filesToShow,
  setFilesToShow,
}: {
  filesToShow: UserFileDto[];
  setFilesToShow: React.Dispatch<React.SetStateAction<UserFileDto[]>>;
}) => {
  const sortKeys = ["date", "name", "size"];
  const [isDesc, setIsDesc] = useState(true);
  const [sortKey, setSortKey] = useState("date");

  useEffect(() => {
    sortFiles(sortKey);
  }, [isDesc]);

  const sortFiles = (e: string) => {
    setSortKey(e);
    if (e === "name" && !isDesc) {
      const sortedByNameAsc = [...filesToShow].sort((a, b) =>
        a.fileName.localeCompare(b.fileName)
      );
      setFilesToShow(sortedByNameAsc);
      return;
    }
    if (e === "name" && isDesc) {
      const sortedByNameDesc = [...filesToShow].sort((a, b) =>
        b.fileName.localeCompare(a.fileName)
      );
      setFilesToShow(sortedByNameDesc);
      return;
    }
    if (e === "size" && !isDesc) {
      const sortedBySizeAsc = [...filesToShow].sort((a, b) => a.size - b.size);
      setFilesToShow(sortedBySizeAsc);
      return;
    }
    if (e === "size" && isDesc) {
      const sortedBySizeDesc = [...filesToShow].sort((a, b) => b.size - a.size);
      setFilesToShow(sortedBySizeDesc);
      return;
    }
    if (e === "date" && !isDesc) {
      const sortedByDateAsc = [...filesToShow].sort(
        (a, b) =>
          new Date(a.createdAt).getTime() - new Date(b.createdAt).getTime()
      );
      setFilesToShow(sortedByDateAsc);
      return;
    }
    if (e === "date" && isDesc) {
      const sortedByDateDesc = [...filesToShow].sort(
        (a, b) =>
          new Date(b.createdAt).getTime() - new Date(a.createdAt).getTime()
      );
      setFilesToShow(sortedByDateDesc);
      return;
    }
  };

  return (
    <div className="p-1 flex w-full md:w-auto md:ml-6 justify-center sm:justify-end items-center gap-1 my-2">
      <select
        name="sort-key"
        id="sort-key"
        className="capitalize p-2 text-base rounded-md bg-white shadow-1"
        onChange={(e) => sortFiles(e.target.value)}
      >
        {sortKeys.map((k) => {
          return (
            <option value={k} key={k} className="capitalize">
              {k}
            </option>
          );
        })}
      </select>
      <div
        onClick={() => {
          setIsDesc((prev) => !prev);
        }}
        className="p-2 bg-white cursor-pointer  shadow-1 rounded-md"
      >
        {isDesc ? <ArrowDownIcon /> : <ArrowUpIcon />}
        <input
          className="hidden"
          type="checkbox"
          name="sort-direction"
          id="sort-direction"
          checked={isDesc}
          onChange={() => setIsDesc((prev) => !prev)}
        />
      </div>
    </div>
  );
};

export default Sort;
