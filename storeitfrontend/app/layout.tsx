import type { Metadata } from "next";
import "./globals.css";

import { config } from "@fortawesome/fontawesome-svg-core";
import "../node_modules/@fortawesome/fontawesome-svg-core/styles.css";
import { AuthProvider } from "../contexts/AuthContext";
config.autoAddCss = false;

export const metadata: Metadata = {
  title: "StoreIt",
  description: "StoreIt - The only storage you need",
};

export default function RootLayout({
  children,
}: Readonly<{
  children: React.ReactNode;
}>) {
  return (
    <html lang="en">
      <body>
        <AuthProvider>{children}</AuthProvider>
      </body>
    </html>
  );
}
