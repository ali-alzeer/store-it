/* eslint-disable @typescript-eslint/no-unused-vars */
/* eslint-disable @typescript-eslint/no-explicit-any */

import React from "react";
import { screen, fireEvent, act } from "@testing-library/react";
import "@testing-library/jest-dom";
import { renderWithAuthContext } from "../test-utils";
import Header from "../components/Header";

// Mocks for Next.js internals
jest.mock("next/navigation", () => ({
  redirect: jest.fn(),
}));
import { redirect } from "next/navigation";

jest.mock("next/link", () => ({
  __esModule: true,
  default: ({ children }: any) => children,
}));
jest.mock("next/image", () => ({
  __esModule: true,
  default: (props: any) => <img {...props} alt={props.alt} />,
}));

jest.mock("../components/FileUploader", () => {
  return function FileUploader() {
    return <div>FileUploader</div>;
  };
});
jest.mock("../components/Search", () => {
  return function Search() {
    return <div>Search</div>;
  };
});

describe("Header", () => {
  afterEach(() => {
    jest.clearAllMocks();
  });

  test("renders header correctly", () => {
    renderWithAuthContext(<Header />);
    expect(screen.getByRole("banner")).toBeInTheDocument();
    expect(screen.getByRole("button")).toBeInTheDocument();
  });

  test("sign out and redirects to sign-in/sign-up page", async () => {
    renderWithAuthContext(<Header />);
    const signOutButton = screen.getByRole("button");
    expect(signOutButton).toBeInTheDocument();
    await act(async () => {
      fireEvent.click(signOutButton);
    });
    expect(redirect).toHaveBeenCalledWith("/");
  });
});
