/* eslint-disable @typescript-eslint/no-explicit-any */

import React from "react";
import { screen, render } from "@testing-library/react";
import user from "@testing-library/user-event";
import "@testing-library/jest-dom";
import MobileNavigation from "../components/MobileNavigation";

// Mocks for Next.js internals
jest.mock("next/navigation", () => ({
  redirect: jest.fn(),
  usePathname: jest.fn(),
}));
import { redirect, usePathname } from "next/navigation";

jest.mock("next/link", () => ({
  __esModule: true,
  default: ({ children }: any) => children,
}));
jest.mock("next/image", () => ({
  __esModule: true,
  default: (props: any) => <img {...props} alt={props.alt} />,
}));

describe("MobileNavigation", () => {
  afterEach(() => {
    jest.clearAllMocks();
    const originalInnerWidth = window.innerWidth;

    Object.defineProperty(window, "innerWidth", {
      writable: true,
      configurable: true,
      value: originalInnerWidth,
    });
    window.dispatchEvent(new Event("resize"));
  });

  test("renders mobile navigation correctly and dashboard is active by default", async () => {
    (usePathname as jest.Mock).mockReturnValue("/");
    Object.defineProperty(window, "innerWidth", {
      writable: true,
      configurable: true,
      value: 500, // Simulate mobile width
    });
    window.dispatchEvent(new Event("resize"));

    render(<MobileNavigation />);

    const MenuTrigger = screen.getByRole("button");
    expect(MenuTrigger).toBeInTheDocument();

    await user.click(MenuTrigger);

    expect(screen.getAllByRole("listitem")).toHaveLength(5);
    expect(screen.getAllByRole("listitem")[0]).toHaveClass("shad-active");
    screen
      .getAllByRole("listitem")
      .slice(1, -1)
      .map((e) => {
        expect(e).not.toHaveClass("shad-active");
      });
  });
  test("changing active listitem depending on pathname", async () => {
    (usePathname as jest.Mock).mockReturnValue("/others");
    Object.defineProperty(window, "innerWidth", {
      writable: true,
      configurable: true,
      value: 500, // Simulate mobile width
    });
    window.dispatchEvent(new Event("resize"));

    render(<MobileNavigation />);

    const MenuTrigger = screen.getByRole("button");
    expect(MenuTrigger).toBeInTheDocument();

    await user.click(MenuTrigger);

    expect(screen.getAllByRole("listitem")).toHaveLength(5);
    expect(screen.getAllByRole("listitem").pop()).toHaveClass("shad-active");
    screen
      .getAllByRole("listitem")
      .slice(0, -2)
      .map((e) => {
        expect(e).not.toHaveClass("shad-active");
      });
  });
  test("sign out and redirects to sign-in/sign-up page", async () => {
    Object.defineProperty(window, "innerWidth", {
      writable: true,
      configurable: true,
      value: 500, // Simulate mobile width
    });
    window.dispatchEvent(new Event("resize"));
    render(<MobileNavigation />);

    const MenuTrigger = screen.getByRole("button");
    expect(MenuTrigger).toBeInTheDocument();

    await user.click(MenuTrigger);

    const signOutButton = screen.getByTestId("signOutButton");
    expect(signOutButton).toBeInTheDocument();

    await user.click(signOutButton);

    expect(redirect).toHaveBeenCalledWith("/");
  });
});
