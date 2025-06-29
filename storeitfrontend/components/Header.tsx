import React, { useContext } from "react";
import { Button } from "../components/ui/button";
import Image from "next/image";
import Search from "../components/Search";
import FileUploader from "../components/FileUploader";
import { saveLoggedInUserToLocalStorage } from "../lib/utils";
import { redirect } from "next/navigation";
import { AuthContext } from "../contexts/AuthContext";

const Header = () => {
  const { user, setUser } = useContext(AuthContext);

  return (
    <header className="header">
      <Search />
      <div className="header-wrapper">
        <FileUploader user={user} />
        <form
          action={async () => {
            saveLoggedInUserToLocalStorage(null);
            setUser(null);
            redirect("/");
          }}
        >
          <Button type="submit" className="sign-out-button">
            <Image
              src="/assets/icons/logout.svg"
              alt="logo"
              width={24}
              height={24}
              className="w-6"
            />
          </Button>
        </form>
      </div>
    </header>
  );
};
export default Header;
