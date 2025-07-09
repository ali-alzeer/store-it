/* eslint-disable @typescript-eslint/no-unused-vars */
import {
  mockFile,
  mockFileOfOtherOwner,
  mockUser,
  renderWithAuthContext,
} from "../test-utils";
import "@testing-library/jest-dom";
import { screen } from "@testing-library/react";
import user from "@testing-library/user-event";

import ActionDropDown from "../components/ActionDropDown";

jest.mock("../lib/actions/file.actions", () => {
  const actual = jest.requireActual("../lib/actions/file.actions");
  return {
    ...actual,
    shareFile: jest.fn((fileShareDto: FileShareDto) => {
      return { success: true, error: null };
    }),
    renameFile: jest.fn((fileRenameDto: FileRenameDto) => {
      return { success: true, error: null };
    }),
    deleteFile: jest.fn((fileDeletionDto: FileDeletionDto) => {
      return { success: true, error: null };
    }),
  };
});
import { deleteFile, renameFile, shareFile } from "../lib/actions/file.actions";

describe("ActionDropDown", () => {
  test("renders UserDetails trigger correctly", async () => {
    renderWithAuthContext(<ActionDropDown file={mockFile} />);
    expect(screen.getByTestId("dropDownTrigger")).toBeInTheDocument();
  });

  test("renders all UserDetails items correctly when the user is the owner of the file", async () => {
    renderWithAuthContext(<ActionDropDown file={mockFile} />);
    const DropDownTrigger = screen.getByTestId("dropDownTrigger");

    await user.click(DropDownTrigger);

    expect(screen.getAllByTestId("dropDownItem")).toHaveLength(5);
  });

  test("renders all UserDetails items except delete item when the user is not the owner of the file", async () => {
    renderWithAuthContext(<ActionDropDown file={mockFileOfOtherOwner} />);
    const DropDownTrigger = screen.getByTestId("dropDownTrigger");

    await user.click(DropDownTrigger);

    expect(screen.getAllByTestId("dropDownItem")).toHaveLength(4);
    screen.getAllByTestId("dropDownItem").map((i) => {
      expect(i).not.toHaveTextContent(/delete/i);
    });
  });

  test("renames file correctly", async () => {
    renderWithAuthContext(<ActionDropDown file={mockFile} />);
    const DropDownTrigger = screen.getByTestId("dropDownTrigger");

    await user.click(DropDownTrigger);

    expect(screen.getAllByTestId("dropDownItem")).toHaveLength(5);
    const RenameItem = screen.getAllByTestId("dropDownItem").find((i) => {
      return i.textContent?.toLowerCase() === "rename";
    });

    expect(RenameItem).toBeInTheDocument();

    await user.click(RenameItem!);

    const RenameDialog = screen.getByTestId("dialogTitle");
    const RenameInput = screen.getByTestId("renameInput");
    const SubmitButton = screen.getByTestId("submitButton");

    expect(RenameDialog).toHaveTextContent("Rename");
    expect(RenameInput).toBeInTheDocument();
    expect(SubmitButton).toBeInTheDocument();

    await user.cut();
    await user.type(RenameInput, "new-name");
    await user.click(SubmitButton);

    const fileRenameDto: FileRenameDto = {
      fileId: mockFile.fileId,
      fileName: "new-name.pdf",
      user: mockUser,
    };

    expect(renameFile as jest.Mock).toHaveBeenCalledWith(fileRenameDto);
  });

  test("shows file details correctly", async () => {
    renderWithAuthContext(<ActionDropDown file={mockFile} />);
    const DropDownTrigger = screen.getByTestId("dropDownTrigger");

    await user.click(DropDownTrigger);

    expect(screen.getAllByTestId("dropDownItem")).toHaveLength(5);
    const DetailsItem = screen.getAllByTestId("dropDownItem").find((i) => {
      return i.textContent?.toLowerCase() === "details";
    });

    expect(DetailsItem).toBeInTheDocument();

    await user.click(DetailsItem!);

    const DetailsDialog = screen.getByTestId("dialogTitle");

    expect(DetailsDialog).toHaveTextContent("Details");
    expect(screen.getByText(mockFile.fileName)).toBeInTheDocument();
  });

  test("shares file correctly", async () => {
    renderWithAuthContext(<ActionDropDown file={mockFile} />);
    const DropDownTrigger = screen.getByTestId("dropDownTrigger");

    await user.click(DropDownTrigger);

    expect(screen.getAllByTestId("dropDownItem")).toHaveLength(5);
    const ShareItem = screen.getAllByTestId("dropDownItem").find((i) => {
      return i.textContent?.toLowerCase() === "share";
    });

    expect(ShareItem).toBeInTheDocument();

    await user.click(ShareItem!);

    const ShareDialog = screen.getByTestId("dialogTitle");
    const ShareInput = screen.getByTestId("shareInput");
    const SubmitButton = screen.getByTestId("submitButton");

    expect(ShareDialog).toHaveTextContent("Share");
    expect(ShareInput).toBeInTheDocument();
    expect(SubmitButton).toBeInTheDocument();

    await user.click(ShareInput);
    await user.type(ShareInput, "adnan@adnan.com");
    await user.click(SubmitButton);

    const fileShareDto: FileShareDto = {
      fileId: mockFile.fileId,
      emails: ["adnan@adnan.com"],
      user: mockUser,
    };

    expect(shareFile as jest.Mock).toHaveBeenCalledWith(fileShareDto);
  });

  test("deletes file correctly", async () => {
    renderWithAuthContext(<ActionDropDown file={mockFile} />);
    const DropDownTrigger = screen.getByTestId("dropDownTrigger");

    await user.click(DropDownTrigger);

    expect(screen.getAllByTestId("dropDownItem")).toHaveLength(5);
    const DeleteItem = screen.getAllByTestId("dropDownItem").find((i) => {
      return i.textContent?.toLowerCase() === "delete";
    });

    expect(DeleteItem).toBeInTheDocument();

    await user.click(DeleteItem!);

    const DeleteDialog = screen.getByTestId("dialogTitle");
    const SubmitButton = screen.getByTestId("submitButton");

    expect(DeleteDialog).toHaveTextContent("Delete");
    expect(SubmitButton).toBeInTheDocument();

    await user.click(SubmitButton);

    const fileDeletionDto: FileDeletionDto = {
      fileId: mockFile.fileId,
      fileUrl: mockFile.url,
      user: mockUser,
    };

    expect(deleteFile as jest.Mock).toHaveBeenCalledWith(fileDeletionDto);
  });
});
