/* eslint-disable @typescript-eslint/no-unused-vars */
import {
  mockUser,
  mockUserWithoutImage,
  renderWithAuthContext,
  renderWithAuthContextNoImage,
} from "../test-utils";
import UserDetails from "../components/UserDetails";
import "@testing-library/jest-dom";
import { screen } from "@testing-library/react";
import user from "@testing-library/user-event";

jest.mock("next/navigation", () => {
  const actual = jest.requireActual("../lib/actions/user.actions");
  return {
    ...actual,
    redirect: jest.fn(() => {}),
  };
});
import { redirect } from "next/navigation";

jest.mock("../lib/actions/user.actions", () => {
  const actual = jest.requireActual("../lib/actions/user.actions");
  return {
    ...actual,
    changeUserImage: jest.fn(() => {
      return { user: mockUser, error: null };
    }),
    deleteUserImage: jest.fn(() => {
      return { user: mockUserWithoutImage, error: null };
    }),
    changeUserName: jest.fn((nameChangeDto: NameChangeDto) => {
      return {
        user: {
          ...mockUser,
          firstName: nameChangeDto.firstName,
          lastName: nameChangeDto.lastName,
        },
        error: null,
      };
    }),
    deleteUserAccount: jest.fn((user: User | null) => {
      return { success: true, error: null };
    }),
  };
});
import {
  changeUserImage,
  changeUserName,
  deleteUserAccount,
  deleteUserImage,
} from "../lib/actions/user.actions";

jest.mock("../lib/utils", () => {
  const actual = jest.requireActual("../lib/utils");
  return {
    ...actual,
    saveLoggedInUserToLocalStorage: jest.fn((user: User | null) => {}),
  };
});
import { saveLoggedInUserToLocalStorage } from "../lib/utils";

const renderUserDetails = (user: User | null) => {
  if (!user) return;

  return (
    <div className="sidebar-user-info cursor-pointer min-h-[60px]">
      <div>UserImage</div>
      <div className="hidden lg:block">
        <p className="subtitle-2 capitalize">
          {user.firstName} {user.lastName}
        </p>
        <p className="caption">{user.email}</p>
      </div>
    </div>
  );
};

describe("UserDetails", () => {
  test("renders UserDetails correctly", async () => {
    renderWithAuthContext(
      <UserDetails renderUserDetails={() => renderUserDetails(mockUser)} />
    );
    expect(screen.getByText("UserImage")).toBeInTheDocument();
    expect(screen.getByText("Ali Alzeer")).toBeInTheDocument();
    expect(screen.getByText("ali.alzeer@example.com")).toBeInTheDocument();
  });

  test("renders Modal with its default values when clicking on UserDetails trigger", async () => {
    renderWithAuthContext(
      <UserDetails renderUserDetails={() => renderUserDetails(mockUser)} />
    );
    const UserDetailsTrigger = screen.getByRole("button");
    await user.click(UserDetailsTrigger);

    expect(screen.getByText("User Details")).toBeInTheDocument();
    expect(screen.getByTestId("changeImageButton")).toBeInTheDocument();
    expect(screen.getByTestId("deleteImageButton")).toBeInTheDocument();
    expect(screen.queryByTestId("changeNameButton")).not.toBeInTheDocument();
    expect(screen.queryByTestId("deleteAccountButton")).not.toBeInTheDocument();
  });

  test("hides deleteImageButton when user has not an image", async () => {
    renderWithAuthContextNoImage(
      <UserDetails
        renderUserDetails={() => renderUserDetails(mockUserWithoutImage)}
      />
    );

    const UserDetailsTrigger = screen.getByRole("button");
    await user.click(UserDetailsTrigger);

    expect(screen.getByText("User Details")).toBeInTheDocument();
    expect(screen.getByTestId("changeImageButton")).toBeInTheDocument();
    expect(screen.queryByTestId("deleteImageButton")).not.toBeInTheDocument();
  });

  test("shows changeNameButton when clicking on changeNameTrigger", async () => {
    renderWithAuthContextNoImage(
      <UserDetails
        renderUserDetails={() => renderUserDetails(mockUserWithoutImage)}
      />
    );

    const UserDetailsTrigger = screen.getByRole("button");
    await user.click(UserDetailsTrigger);

    expect(screen.getByText("User Details")).toBeInTheDocument();
    expect(screen.queryByTestId("changeNameButton")).not.toBeInTheDocument();

    const ChangeNameTrigger = screen.getByTestId("changeNameTrigger");
    await user.click(ChangeNameTrigger);

    expect(screen.getByTestId("changeNameButton")).toBeInTheDocument();
  });

  test("shows deleteAccountButton when clicking on deleteAccountTrigger", async () => {
    renderWithAuthContextNoImage(
      <UserDetails
        renderUserDetails={() => renderUserDetails(mockUserWithoutImage)}
      />
    );

    const UserDetailsTrigger = screen.getByRole("button");
    await user.click(UserDetailsTrigger);

    expect(screen.getByText("User Details")).toBeInTheDocument();
    expect(screen.queryByTestId("deleteAccountButton")).not.toBeInTheDocument();

    const ChangeNameTrigger = screen.getByTestId("deleteAccountTrigger");
    await user.click(ChangeNameTrigger);

    expect(screen.getByTestId("deleteAccountButton")).toBeInTheDocument();
  });

  test("changes image correctly", async () => {
    renderWithAuthContextNoImage(
      <UserDetails
        renderUserDetails={() => renderUserDetails(mockUserWithoutImage)}
      />
    );

    const UserDetailsTrigger = screen.getByRole("button");
    await user.click(UserDetailsTrigger);

    const ChangeImageButton = screen.getByTestId("changeImageButton");
    const ImageInput = screen.getByTestId("imageInput");

    expect(screen.getByText("User Details")).toBeInTheDocument();
    expect(ChangeImageButton).toBeInTheDocument();
    expect(ImageInput).toBeInTheDocument();

    const imageFile = new File(["dummy content"], "test-image.png", {
      type: "image/png",
    });

    await user.click(ChangeImageButton);
    await user.upload(ImageInput, imageFile);

    expect(changeUserImage as jest.Mock).toHaveBeenCalled();
    expect(saveLoggedInUserToLocalStorage as jest.Mock).toHaveBeenCalled();
  });

  test("deletes image correctly", async () => {
    renderWithAuthContext(
      <UserDetails renderUserDetails={() => renderUserDetails(mockUser)} />
    );

    const UserDetailsTrigger = screen.getByRole("button");
    await user.click(UserDetailsTrigger);

    const DeleteImageButton = screen.getByTestId("deleteImageButton");

    expect(screen.getByText("User Details")).toBeInTheDocument();
    expect(DeleteImageButton).toBeInTheDocument();

    await user.click(DeleteImageButton);

    expect(deleteUserImage as jest.Mock).toHaveBeenCalled();
    expect(saveLoggedInUserToLocalStorage as jest.Mock).toHaveBeenCalled();
  });

  test("changes name correctly", async () => {
    renderWithAuthContext(
      <UserDetails renderUserDetails={() => renderUserDetails(mockUser)} />
    );

    const UserDetailsTrigger = screen.getByRole("button");
    await user.click(UserDetailsTrigger);
    expect(screen.getByText("User Details")).toBeInTheDocument();

    const ChangeNameTrigger = screen.getByTestId("changeNameTrigger");
    expect(ChangeNameTrigger).toBeInTheDocument();

    await user.click(ChangeNameTrigger);

    const ChangeNameButton = screen.getByTestId("changeNameButton");
    expect(ChangeNameButton).toBeInTheDocument();

    const FirstNameInput = screen.getByPlaceholderText("First name");
    const LastNameInput = screen.getByPlaceholderText("Last name");

    await user.type(FirstNameInput, "Ahmad");
    await user.type(LastNameInput, "Adnan");
    await user.click(ChangeNameButton);

    const NewNameChangeDto: NameChangeDto = {
      firstName: "Ahmad",
      lastName: "Adnan",
      user: mockUser,
    };

    expect(changeUserName as jest.Mock).toHaveBeenCalledWith(NewNameChangeDto);
    expect(saveLoggedInUserToLocalStorage as jest.Mock).toHaveBeenCalled();
  });

  test("deletes account correctly", async () => {
    renderWithAuthContext(
      <UserDetails renderUserDetails={() => renderUserDetails(mockUser)} />
    );

    const UserDetailsTrigger = screen.getByRole("button");
    await user.click(UserDetailsTrigger);
    expect(screen.getByText("User Details")).toBeInTheDocument();

    const DeleteAccountTrigger = screen.getByTestId("deleteAccountTrigger");
    expect(DeleteAccountTrigger).toBeInTheDocument();

    await user.click(DeleteAccountTrigger);

    const DeleteAccountButton = screen.getByTestId("deleteAccountButton");
    expect(DeleteAccountButton).toBeInTheDocument();

    await user.click(DeleteAccountButton);

    expect(deleteUserAccount as jest.Mock).toHaveBeenCalledWith(mockUser);
    expect(saveLoggedInUserToLocalStorage as jest.Mock).toHaveBeenCalled();
    expect(redirect).toHaveBeenCalledWith("/");
  });
});
