"use client";

import { createContext, useState, useEffect, useContext } from "react";
import { AuthContext } from "./AuthContext";
import { getAllFilesForUser } from "../lib/actions/file.actions";

export const FilesContext = createContext<FilesContextType>({
  files: null,
  setFiles: () => {},
  notification: false,
  setNotification: () => {},
});

export const FilesProvider = ({ children }: FilesProviderProps) => {
  const { user } = useContext(AuthContext);

  const [files, setFiles] = useState<UserFileDto[] | null>(null);
  const [notification, setNotification] = useState<boolean>(false);
  const [isLoading, setIsLoading] = useState<boolean>(true);

  useEffect(() => {
    if (!user) {
      setFiles(null);
      setIsLoading(false);
      return;
    }

    const getFiles = async () => {
      setIsLoading(true);
      try {
        const filesErrorObject = await getAllFilesForUser(user);
        setFiles(filesErrorObject.files ?? []);
      } catch (error) {
        console.log("Error occurred : ", error);
        setFiles([]);
      } finally {
        setIsLoading(false);
      }
    };

    getFiles();

    return () => {};
  }, [notification]);

  if (isLoading) {
    return (
      <FilesContext.Provider
        value={{ files, setFiles, notification, setNotification }}
      >
        <div className="w-dvw h-dvh flex flex-col justify-center items-center gap-3">
          <div className="w-20 h-20 border-4 border-transparent border-t-brand animate-spin rounded-full"></div>
        </div>
      </FilesContext.Provider>
    );
  } else {
    return (
      <FilesContext.Provider
        value={{ files, setFiles, notification, setNotification }}
      >
        {children}
      </FilesContext.Provider>
    );
  }
};
