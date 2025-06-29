"use client";

import { useContext, useEffect, useState } from "react";
import { useParams } from "next/navigation";
import {
  calculatePercentage,
  calculateTotalFileSize,
  convertFileSize,
  filterFilesAccordingToParams,
  isValidPageName,
  titleCase,
} from "../lib/utils";
import Sort from "./Sort";
import Card from "./Card";
import { MAX_CAPACITY_SIZE_FOR_USER } from "../constants";
import { FilesContext } from "../contexts/FilesContext";
import CircularProgressShapeDemo from "./ui/progress-11";
import LinearProgressWithLabelDemo from "./ui/progress-02";

export default function HomePage() {
  const [filesToShow, setFilesToShow] = useState<UserFileDto[]>([]);
  const [isLoading, setIsLoading] = useState<boolean>(false);
  const params = useParams();
  const { files, notification } = useContext(FilesContext);

  useEffect(() => {
    if (files === null) return;
    setIsLoading(true);
    setFilesToShow(filterFilesAccordingToParams(params.pageName, files));
    setTimeout(() => {
      setIsLoading(false);
    }, 200);
  }, [files, notification, params]);

  return (
    <>
      {isLoading ? (
        <>
          <div className="bg-gray-200 p-5 rounded-4xl h-full overflow-hidden">
            <div className="w-full h-full flex flex-col justify-center items-center gap-3 bg-gray-200">
              <div className="w-20 h-20 border-4 border-transparent border-t-brand animate-spin rounded-full"></div>
            </div>
          </div>
        </>
      ) : (
        <>
          <div className="bg-gray-200 p-5 rounded-4xl h-full overflow-hidden">
            <div className="flex flex-col justify-between items-start px-2 pb-4 gap-2">
              <h1 className="h1 text-black">
                {params.pageName && isValidPageName(params.pageName.toString())
                  ? titleCase(params.pageName.toString())
                  : "Dashboard"}
              </h1>
              <div className="w-full text-xs sm:text-sm md:text-base flex flex-col md:flex-row justify-between items-start md:items-center">
                {params.pageName !== "documents" &&
                params.pageName !== "media" &&
                params.pageName !== "images" &&
                params.pageName !== "others" ? (
                  <div className="rounded-lg bg-brand flex justify-center items-center w-full md:justify-between lg:py-2">
                    <p className="w-3/4 min-w-3/4 lg:w-1/2 lg:min-w-1/2 text-white text-center md:text-start pl-5">
                      Total {" (all files) "} :{" "}
                      <span className="font-bold">
                        {convertFileSize(calculateTotalFileSize(files ?? []))}
                        <span className="font-medium">{" / "}</span>
                        {convertFileSize(MAX_CAPACITY_SIZE_FOR_USER)}
                      </span>
                    </p>
                    {window.innerWidth > 1024 ? (
                      <LinearProgressWithLabelDemo
                        progress={calculatePercentage(
                          calculateTotalFileSize(files ?? [])
                        )}
                      />
                    ) : (
                      <CircularProgressShapeDemo
                        value={calculatePercentage(
                          calculateTotalFileSize(files ?? [])
                        )}
                        circleStrokeWidth={5}
                        progressStrokeWidth={5}
                        strokeWidth={5}
                        shape="square"
                        size={80}
                        className="w-1/4 min-w-1/4"
                      />
                    )}
                  </div>
                ) : (
                  <p>
                    Total {` (${params.pageName}) `} :{" "}
                    <span className="font-bold">
                      {convertFileSize(
                        calculateTotalFileSize(filesToShow ?? 0)
                      )}
                    </span>
                  </p>
                )}
                <Sort
                  filesToShow={filesToShow}
                  setFilesToShow={setFilesToShow}
                />
              </div>
            </div>
            {files === null || files.length === 0 ? (
              <div className="flex h-[70%] w-full justify-center items-center gap-2 flex-col">
                <h1 className="h2 text-center">
                  <span className="h1 p-2">StoreIt</span>
                  <br></br>
                  The only storage solution you need<br></br>
                  <hr className="my-1" /> Start uploading now
                </h1>
              </div>
            ) : (
              <div className="dashboard-container overflow-y-scroll bg-gray-400 rounded-3xl p-2">
                {filesToShow.map((f) => {
                  return <Card file={f} key={`${f.fileId}-${f.ownerId}`} />;
                })}
              </div>
            )}
          </div>
        </>
      )}
    </>
  );
}
