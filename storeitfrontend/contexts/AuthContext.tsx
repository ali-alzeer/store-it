"use client";

import { createContext, useState, useEffect } from "react";
import { isUser, saveLoggedInUserToLocalStorage } from "../lib/utils";
import { validateJwtToken } from "../lib/actions/user.actions";

export const AuthContext = createContext<AuthContextType>({
  user: null,
  setUser: () => {},
});

export const AuthProvider = ({ children }: AuthProviderProps) => {
  const [user, setUser] = useState<User | null>(null);
  const [isLoading, setIsLoading] = useState<boolean>(true);
  useEffect(() => {
    const validateToken = async () => {
      setIsLoading(true);
      const storedUser = localStorage.getItem("user");
      if (storedUser) {
        try {
          const parsedUser = JSON.parse(storedUser);
          // Verify that the parsed object matches our User type
          if (isUser(parsedUser)) {
            // Validate the JWT token

            let isValidToken = false;

            const jwtDto: JwtDto = {
              token: parsedUser.jwt,
            };

            isValidToken = await validateJwtToken(jwtDto);

            if (isValidToken) {
              setIsLoading(false);
              saveLoggedInUserToLocalStorage(parsedUser);
              setUser(parsedUser);
            } else {
              setIsLoading(false);
              saveLoggedInUserToLocalStorage(null);
              setUser(null);
            }
          } else {
            setIsLoading(false);
            saveLoggedInUserToLocalStorage(null);
            setUser(null);
          }
        } catch (error) {
          setIsLoading(false);
          console.error("Error parsing stored user:", error);
          saveLoggedInUserToLocalStorage(null);
          setUser(null);
        }
      } else {
        setIsLoading(false);
        saveLoggedInUserToLocalStorage(null);
        setUser(null);
      }
    };

    validateToken();
  }, []);

  if (isLoading) {
    return (
      <AuthContext.Provider value={{ user, setUser }}>
        <div className="w-dvw h-dvh flex flex-col justify-center items-center gap-3">
          <div className="w-20 h-20 border-4 border-transparent border-t-brand animate-spin rounded-full"></div>
        </div>
      </AuthContext.Provider>
    );
  } else {
    return (
      <AuthContext.Provider value={{ user, setUser }}>
        {children}
      </AuthContext.Provider>
    );
  }
};
