import { ReactElement } from "react";
import { render, RenderOptions } from "@testing-library/react";
import { FilesContext } from "./contexts/FilesContext";
import { AuthContext } from "./contexts/AuthContext";

const mockUser: User = {
  id: "550e8400-e29b-41d4-a716-446655440000",
  firstName: "Ali",
  lastName: "Alzeer",
  email: "ali.alzeer@example.com",
  imageUrl: "https://valid-image-url.png",
  createdAt: new Date("2025-07-05T08:30:00Z"),
  jwt: "",
};

const mockUserWithoutImage: User = {
  id: "550e8400-e29b-41d4-a716-446655440000",
  firstName: "Ali",
  lastName: "Alzeer",
  email: "ali.alzeer@example.com",
  imageUrl: null,
  createdAt: new Date("2025-07-05T08:30:00Z"),
  jwt: "",
};

const mockFile: UserFileDto = {
  fileId: "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  fileName: "project-plan.pdf",
  url: "https://fake-url.com/files/project-plan.pdf",
  typeId: "1c9d9f3e-4f2b-4f3e-9c2e-8f3e9f3e9f3e",
  fileTypeName: "Document",
  extensionId: "2a5c9f3e-4f2b-4f3e-9c2e-8f3e9f3e9f3e",
  fileExtensionName: "pdf",
  size: 1048576,
  createdAt: new Date("2025-07-08T19:30:00Z"),
  ownerId: mockUser.id,
  ownerFirstName: mockUser.firstName,
  ownerLastName: mockUser.lastName,
  ownerImageUrl: mockUser.imageUrl ?? "",
};

const mockFileOfOtherOwner: UserFileDto = {
  fileId: "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  fileName: "project-plan.pdf",
  url: "https://fake-url.com/files/project-plan.pdf",
  typeId: "1c9d9f3e-4f2b-4f3e-9c2e-8f3e9f3e9f3e",
  fileTypeName: "Document",
  extensionId: "2a5c9f3e-4f2b-4f3e-9c2e-8f3e9f3e9f3e",
  fileExtensionName: "pdf",
  size: 1048576,
  createdAt: new Date("2025-07-08T19:30:00Z"),
  ownerId: "2b5c9f3a-4f2b-4f3e-9c2e-8f3e9f3e9faf",
  ownerFirstName: "Ahmad",
  ownerLastName: "Adnan",
  ownerImageUrl: "",
};

const AuthProviderWrapper = ({ children }: { children: React.ReactNode }) => {
  return (
    <AuthContext.Provider
      value={{
        user: mockUser,
        setUser: () => {},
      }}
    >
      {children}
    </AuthContext.Provider>
  );
};

const renderWithAuthContext = (
  ui: ReactElement,
  options?: Omit<RenderOptions, "wrapper">
) => render(ui, { wrapper: AuthProviderWrapper, ...options });

const AuthNoImageProviderWrapper = ({
  children,
}: {
  children: React.ReactNode;
}) => {
  return (
    <AuthContext.Provider
      value={{
        user: mockUserWithoutImage,
        setUser: () => {},
      }}
    >
      {children}
    </AuthContext.Provider>
  );
};

const renderWithAuthContextNoImage = (
  ui: ReactElement,
  options?: Omit<RenderOptions, "wrapper">
) => render(ui, { wrapper: AuthNoImageProviderWrapper, ...options });

const UnAuthProviderWrapper = ({ children }: { children: React.ReactNode }) => {
  return (
    <AuthContext.Provider value={{ user: null, setUser: () => {} }}>
      {children}
    </AuthContext.Provider>
  );
};

const renderWithUnAuthContext = (
  ui: ReactElement,
  options?: Omit<RenderOptions, "wrapper">
) => render(ui, { wrapper: UnAuthProviderWrapper, ...options });

const FilesProviderWrapper = ({ children }: { children: React.ReactNode }) => {
  return (
    <FilesContext.Provider
      value={{
        files: [
          {
            fileId: "f1a2b3c4-d5e6-7890-abcd-1234567890ef",
            fileName: "ProjectProposal.pdf",
            url: "https://example.com/files/f1a2b3c4-d5e6-7890-abcd-1234567890ef",
            typeId: "type-001",
            fileTypeName: "Document",
            extensionId: "ext-pdf",
            fileExtensionName: "PDF",
            size: 245678,
            createdAt: new Date("2024-11-15T10:23:00Z"),
            ownerId: "user-123",
            ownerFirstName: "Layla",
            ownerLastName: "Hassan",
            ownerImageUrl: "https://example.com/profiles/user-123.jpg",
          },

          {
            fileId: "a9b8c7d6-e5f4-3210-fedc-0987654321ba",
            fileName: "TeamPhoto.png",
            url: "https://example.com/files/a9b8c7d6-e5f4-3210-fedc-0987654321ba",
            typeId: "type-002",
            fileTypeName: "Image",
            extensionId: "ext-png",
            fileExtensionName: "PNG",
            size: 1048576,
            createdAt: new Date("2025-03-22T14:45:30Z"),
            ownerId: "user-456",
            ownerFirstName: "Omar",
            ownerLastName: "Nasser",
            ownerImageUrl: "https://example.com/profiles/user-456.jpg",
          },
        ],
        setFiles: () => {},
        notification: false,
        setNotification: () => {},
      }}
    >
      {children}
    </FilesContext.Provider>
  );
};

const renderWithFilesContext = (
  ui: ReactElement,
  options?: Omit<RenderOptions, "wrapper">
) => render(ui, { wrapper: FilesProviderWrapper, ...options });

export * from "@testing-library/react";
export {
  mockUser,
  mockUserWithoutImage,
  mockFile,
  mockFileOfOtherOwner,
  renderWithAuthContext,
  renderWithAuthContextNoImage,
  renderWithUnAuthContext,
  renderWithFilesContext,
};
