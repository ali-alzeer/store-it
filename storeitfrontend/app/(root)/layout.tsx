"use client";

import React, { useContext } from "react";
import Sidebar from "../../components/Sidebar";
import MobileNavigation from "../../components/MobileNavigation";
import Header from "../../components/Header";
import { redirect } from "next/navigation";
import { Toaster } from "../../components/ui/toaster";
import { AuthContext } from "../../contexts/AuthContext";
import { FilesProvider } from "../../contexts/FilesContext";

const Layout = ({ children }: { children: React.ReactNode }) => {
  const { user } = useContext(AuthContext);

  if (user !== undefined && user !== null) {
    return (
      <FilesProvider>
        <main className="flex h-screen">
          <Sidebar {...user} />
          <section className="flex h-full flex-1 flex-col">
            <MobileNavigation />
            <Header />
            <div className="main-content">{children}</div>
          </section>
          <Toaster />
        </main>
      </FilesProvider>
    );
  } else {
    return redirect("/sign-in");
  }
};
export default Layout;
