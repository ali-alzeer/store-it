/* eslint-disable @typescript-eslint/no-unused-vars */
/* eslint-disable @typescript-eslint/no-explicit-any */

import React from "react";
import { screen } from "@testing-library/react";
import "@testing-library/jest-dom";
import { mockUser, renderWithAuthContext } from "../test-utils";
import Sidebar from "../components/Sidebar";

// Mocks for Next.js internals
jest.mock("next/navigation", () => ({
  redirect: jest.fn(),
  usePathname: jest.fn(),
}));
import { usePathname } from "next/navigation";
jest.mock("next/link", () => ({
  __esModule: true,
  default: ({ children }: any) => children,
}));
jest.mock("next/image", () => ({
  __esModule: true,
  default: (props: any) => <img {...props} alt={props.alt} />,
}));

describe("Sidebar", () => {
  afterEach(() => {
    jest.clearAllMocks();
  });

  test("renders sidebar correctly and dashboard is active by default", () => {
    (usePathname as jest.Mock).mockReturnValue("/");

    renderWithAuthContext(
      <Sidebar
        id={mockUser.id}
        createdAt={mockUser.createdAt}
        email={mockUser.email}
        firstName={mockUser.firstName}
        lastName={mockUser.lastName}
        imageUrl={mockUser.imageUrl}
        jwt={mockUser.jwt}
      />
    );

    expect(screen.getAllByRole("listitem")).toHaveLength(5);
    expect(screen.getAllByRole("listitem")[0]).toHaveClass("shad-active");
    screen
      .getAllByRole("listitem")
      .slice(1, -1)
      .map((e) => {
        expect(e).not.toHaveClass("shad-active");
      });
  });
  test("changing active listitem depending on pathname", () => {
    (usePathname as jest.Mock).mockReturnValue("/others");

    renderWithAuthContext(
      <Sidebar
        id={mockUser.id}
        createdAt={mockUser.createdAt}
        email={mockUser.email}
        firstName={mockUser.firstName}
        lastName={mockUser.lastName}
        imageUrl={mockUser.imageUrl}
        jwt={mockUser.jwt}
      />
    );

    expect(screen.getAllByRole("listitem")).toHaveLength(5);
    expect(screen.getAllByRole("listitem").pop()).toHaveClass("shad-active");
    screen
      .getAllByRole("listitem")
      .slice(0, -2)
      .map((e) => {
        expect(e).not.toHaveClass("shad-active");
      });
  });
});
