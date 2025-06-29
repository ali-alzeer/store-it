"use client";

import {
  Sheet,
  SheetContent,
  SheetTitle,
  SheetTrigger,
} from "../components/ui/sheet";
import Image from "next/image";
import React, { useContext, useState } from "react";
import { redirect, usePathname } from "next/navigation";
import { Separator } from "@radix-ui/react-separator";
import { navItems } from "../constants";
import { cn, saveLoggedInUserToLocalStorage } from "../lib/utils";
import { Button } from "../components/ui/button";
import FileUploader from "../components/FileUploader";
import { User2Icon } from "lucide-react";
import { AuthContext } from "../contexts/AuthContext";
import UserDetails from "./UserDetails";

const MobileNavigation = () => {
  const { user, setUser } = useContext(AuthContext);

  const [open, setOpen] = useState(false);

  const pathname = usePathname();

  const renderUserDetails = (user: User | null) => {
    if (!user) return;

    return (
      <div className="header-user cursor-pointer min-h-[60px]">
        {user!.imageUrl ? (
          <Image
            src={user!.imageUrl!}
            alt="Avatar"
            width={44}
            height={44}
            className="sidebar-user-avatar"
          />
        ) : (
          <User2Icon className="sidebar-user-avatar" />
        )}
        <div className="text-start sm:hidden lg:block">
          <p className="subtitle-2 capitalize">
            {user!.firstName} {user!.lastName}
          </p>
          <p className="caption">{user!.email}</p>
        </div>
      </div>
    );
  };

  return (
    <header className="mobile-header">
      <Image
        src="/assets/icons/logo-full-brand.svg"
        alt="logo"
        width={120}
        height={52}
        className="h-auto"
      />

      <Sheet open={open} onOpenChange={setOpen}>
        <SheetTrigger>
          <Image
            src="/assets/icons/menu.svg"
            alt="Search"
            width={30}
            height={30}
          />
        </SheetTrigger>
        <SheetContent className="shad-sheet h-screen px-3">
          <SheetTitle>
            <UserDetails renderUserDetails={() => renderUserDetails(user)} />
            <Separator className="mb-4 bg-light-200/20" />
          </SheetTitle>

          <nav className="mobile-nav">
            <ul className="mobile-nav-list">
              {navItems.map(({ url, name, icon }) => (
                <div
                  key={name}
                  onClick={() => {
                    redirect(url);
                  }}
                  className="cursor-pointer lg:w-full"
                >
                  <li
                    className={cn(
                      "mobile-nav-item",
                      pathname === url && "shad-active"
                    )}
                  >
                    <Image
                      src={icon}
                      alt={name}
                      width={24}
                      height={24}
                      className={cn(
                        "nav-icon",
                        pathname === url && "nav-icon-active"
                      )}
                    />
                    <p>{name}</p>
                  </li>
                </div>
              ))}
            </ul>
          </nav>

          <Separator className="my-5 bg-light-200/20" />

          <div className="flex flex-col justify-between gap-5 pb-5">
            <FileUploader user={user} />
            <Button
              type="submit"
              className="mobile-sign-out-button text-start flex justify-start"
              onClick={async () => {
                saveLoggedInUserToLocalStorage(null);
                setUser(null);
                redirect("/");
              }}
            >
              <Image
                src="/assets/icons/logout.svg"
                alt="logo"
                width={24}
                height={24}
              />
              <p>Logout</p>
            </Button>
          </div>
        </SheetContent>
      </Sheet>
    </header>
  );
};

export default MobileNavigation;
