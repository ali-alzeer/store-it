import { ReactElement } from "react";
import { render, RenderOptions } from "@testing-library/react";
import { FilesContext } from "./contexts/FilesContext";
import { AuthContext } from "./contexts/AuthContext";

const AuthProviderWrapper = ({ children }: { children: React.ReactNode }) => {
  return (
    <AuthContext.Provider
      value={{
        user: {
          id: "550e8400-e29b-41d4-a716-446655440000",
          firstName: "Amina",
          lastName: "Haddad",
          email: "amina.haddad@example.com",
          imageUrl: "https://example.com/avatars/amina-haddad.png",
          createdAt: new Date("2025-07-05T08:30:00Z"),
          jwt: [
            "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9",
            "eyJ1c2VySWQiOiI1NTBlODQwMC1lMjliLTQxZDQtYTcxNi00NDY2NTU0NDAwMDAiLCJpYXQiOjE2ODY5ODQ4MDB9",
            "dBjftJeZ4CVP-mB92K27uhbUJU1p1r_wW1gFWFOEjXk",
          ].join("."),
        },

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
  renderWithAuthContext,
  renderWithUnAuthContext,
  renderWithFilesContext,
};
