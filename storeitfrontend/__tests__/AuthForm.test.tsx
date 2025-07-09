/* eslint-disable @typescript-eslint/no-unused-vars */
/* eslint-disable @typescript-eslint/no-explicit-any */

import React from "react";
import { screen, fireEvent, waitFor } from "@testing-library/react";
import "@testing-library/jest-dom";
import AuthForm from "../components/AuthForm";
import { signInUser, createAccount } from "../lib/actions/user.actions";
import * as navigation from "next/navigation";
import { renderWithAuthContext, renderWithUnAuthContext } from "../test-utils";

// Mocks for Next.js internals
jest.mock("next/navigation", () => ({ redirect: jest.fn() }));
jest.mock("next/link", () => ({
  __esModule: true,
  default: ({ children }: any) => children,
}));
jest.mock("next/image", () => ({
  __esModule: true,
  default: (props: any) => <img {...props} alt={props.alt} />,
}));

jest.mock("../lib/actions/user.actions", () => ({
  __esModule: true,
  createAccount: jest.fn(),
  signInUser: jest.fn(),
}));

jest.mock("../lib/utils", () => {
  const actual = jest.requireActual("../lib/utils");
  return {
    __esModule: true,
    ...actual,
    saveLoggedInUserToLocalStorage: jest.fn((user: User | null) => {}),
  };
});
import { saveLoggedInUserToLocalStorage } from "../lib/utils";

describe("AuthForm — Sign In", () => {
  afterEach(() => {
    jest.clearAllMocks();
  });

  test("renders email and password fields and Sign In button", () => {
    renderWithUnAuthContext(<AuthForm type="sign-in" />);

    expect(screen.getByLabelText(/email/i)).toBeInTheDocument();
    expect(
      screen.getByPlaceholderText("Enter your password")
    ).toBeInTheDocument();
    expect(
      screen.getByRole("button", { name: /sign in/i })
    ).toBeInTheDocument();
  });

  test("toggles password visibility", () => {
    renderWithUnAuthContext(<AuthForm type="sign-in" />);

    const passwordInput = screen.getByPlaceholderText("Enter your password");
    const toggle = screen.getByText(/show/i);

    // initially password type is password
    expect(passwordInput).toHaveAttribute("type", "password");

    fireEvent.click(toggle);
    expect(passwordInput).toHaveAttribute("type", "text");
    expect(toggle).toHaveTextContent(/hide/i);

    fireEvent.click(toggle);
    expect(passwordInput).toHaveAttribute("type", "password");
    expect(toggle).toHaveTextContent(/show/i);
  });

  test("submits successfully and redirects", async () => {
    const mockUser: User = {
      id: "550e8400-e29b-41d4-a716-446655440000",
      firstName: "Ali",
      lastName: "Alzeer",
      email: "ali.alzeer@example.com",
      imageUrl: "",
      createdAt: new Date("2025-07-05T08:30:00Z"),
      jwt: "",
    };
    (signInUser as jest.Mock).mockResolvedValue({
      user: mockUser,
      error: null,
    });

    renderWithUnAuthContext(<AuthForm type="sign-in" />);

    fireEvent.input(screen.getByLabelText(/email/i), {
      target: { value: "ali@alzeer.com" },
    });
    fireEvent.input(screen.getByPlaceholderText("Enter your password"), {
      target: { value: "password123" },
    });

    const btn = screen.getByRole("button", { name: /sign in/i });
    fireEvent.click(btn);

    await waitFor(() => {
      expect(signInUser).toHaveBeenCalledWith({
        email: "ali@alzeer.com",
        password: "password123",
      });
      expect(saveLoggedInUserToLocalStorage).toHaveBeenCalledWith(mockUser);
      expect(navigation.redirect).toHaveBeenCalledWith("/");
    });
  });

  test("displays error message on failure", async () => {
    (signInUser as jest.Mock).mockResolvedValue({
      user: null,
      error: "Invalid credentials",
    });

    renderWithUnAuthContext(<AuthForm type="sign-in" />);

    fireEvent.input(screen.getByLabelText(/email/i), {
      target: { value: "bad@user.com" },
    });
    fireEvent.input(screen.getByPlaceholderText("Enter your password"), {
      target: { value: "short" },
    });

    fireEvent.click(screen.getByRole("button", { name: /sign in/i }));

    expect(
      await screen.findByText("String must contain at least 6 character(s)")
    ).toBeInTheDocument();

    fireEvent.input(screen.getByLabelText(/email/i), {
      target: { value: "notexist@user.com" },
    });
    fireEvent.input(screen.getByPlaceholderText("Enter your password"), {
      target: { value: "notexistpassword" },
    });

    fireEvent.click(screen.getByRole("button", { name: /sign in/i }));

    expect(await screen.findByText("*Invalid credentials")).toBeInTheDocument();

    expect(saveLoggedInUserToLocalStorage).toHaveBeenCalledWith(null);
  });
});

describe("AuthForm — Sign Up", () => {
  afterEach(() => {
    jest.clearAllMocks();
  });

  test("renders all fields for sign-up", () => {
    renderWithUnAuthContext(<AuthForm type="sign-up" />);

    expect(screen.getByLabelText(/first name/i)).toBeInTheDocument();
    expect(screen.getByLabelText(/last name/i)).toBeInTheDocument();
    expect(screen.getByLabelText(/email/i)).toBeInTheDocument();
    expect(
      screen.getByPlaceholderText("Enter your password")
    ).toBeInTheDocument();
    expect(
      screen.getByRole("button", { name: /sign up/i })
    ).toBeInTheDocument();
  });

  test("submits sign-up and calls createAccount", async () => {
    const newUser = {
      id: "550e8400-e29b-41d4-a716-446655440000",
      firstName: "Ali",
      lastName: "Alzeer",
      email: "ali.alzeer@example.com",
      imageUrl: "",
      createdAt: new Date("2025-07-05T08:30:00Z"),
      jwt: "",
    };
    (createAccount as jest.Mock).mockResolvedValue({
      user: newUser,
      error: null,
    });

    renderWithUnAuthContext(<AuthForm type="sign-up" />);

    fireEvent.input(screen.getByLabelText(/first name/i), {
      target: { value: "Ali" },
    });
    fireEvent.input(screen.getByLabelText(/last name/i), {
      target: { value: "Alzeer" },
    });
    fireEvent.input(screen.getByLabelText(/email/i), {
      target: { value: "ali.alzeer@example.com" },
    });
    fireEvent.input(screen.getByPlaceholderText("Enter your password"), {
      target: { value: "securePass1" },
    });

    fireEvent.click(screen.getByRole("button", { name: /sign up/i }));

    await waitFor(() => {
      expect(createAccount).toHaveBeenCalledWith({
        firstName: "Ali",
        lastName: "Alzeer",
        email: "ali.alzeer@example.com",
        password: "securePass1",
      });
      expect(navigation.redirect).toHaveBeenCalledWith("/");
    });
  });
});

describe("AuthForm — Redirect when already logged in", () => {
  afterEach(() => {
    jest.clearAllMocks();
  });

  test("immediately redirects if user context is non-null", () => {
    renderWithAuthContext(<AuthForm type="sign-in" />);

    expect(navigation.redirect).toHaveBeenCalledWith("/");
  });
});
