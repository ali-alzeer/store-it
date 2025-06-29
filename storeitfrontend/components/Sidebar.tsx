"use client";

import Link from "next/link";
import Image from "next/image";
import { navItems } from "../constants";
import { usePathname } from "next/navigation";
import { cn } from "../lib/utils";
import { User2Icon } from "lucide-react";
import UserDetails from "./UserDetails";

const Sidebar = (user: User) => {
  const pathname = usePathname();

  const renderUserDetails = (user: User | null) => {
    if (!user) return;

    return (
      <div className="sidebar-user-info cursor-pointer min-h-[60px]">
        {user.imageUrl ? (
          <Image
            src={user.imageUrl!}
            alt="Avatar"
            width={44}
            height={44}
            className="sidebar-user-avatar"
          />
        ) : (
          <User2Icon className="sidebar-user-avatar" />
        )}

        <div className="hidden lg:block">
          <p className="subtitle-2 capitalize">
            {user.firstName} {user.lastName}
          </p>
          <p className="caption">{user.email}</p>
        </div>
      </div>
    );
  };

  return (
    <>
      <aside className="sidebar">
        <Link href="/">
          <Image
            src="/assets/icons/logo-full-brand.svg"
            alt="logo"
            width={160}
            height={50}
            className="hidden h-auto lg:block"
          />

          <Image
            src="/assets/icons/logo-brand.svg"
            alt="logo"
            width={52}
            height={52}
            className="lg:hidden"
          />
        </Link>

        <nav className="sidebar-nav">
          <ul className="flex flex-1 flex-col gap-6">
            {navItems.map(({ url, name, icon }) => (
              <Link key={name} href={url} className="lg:w-full">
                <li
                  className={cn(
                    "sidebar-nav-item",
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
                  <p className="hidden lg:block">{name}</p>
                </li>
              </Link>
            ))}
          </ul>
        </nav>
        <UserDetails renderUserDetails={() => renderUserDetails(user)} />
      </aside>
    </>
  );
};
export default Sidebar;
