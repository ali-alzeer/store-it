// USER

interface AuthProviderProps {
  children: ReactNode;
}

interface AuthContextType {
  user: User | null;
  setUser: (user: User | null) => void;
}

declare type User = {
  id: string;
  firstName: string;
  lastName: string;
  email: string;
  imageUrl: string | null;
  createdAt: Date;
  jwt: string;
};

declare type SignInDto = {
  email: string;
  password: string;
};

declare type SignUpDto = {
  firstName: string;
  lastName: string;
  email: string;
  password: string;
};

declare type JwtDto = {
  token: string;
};

declare enum TokenValidationErrorType {
  Invalid = 1,
  Expired = 2,
}

declare type TokenValidation = {
  isValid: boolean;
  TokenValidation: TokenValidationErrorType | null;
};

// FILE

declare type FileType = "document" | "image" | "video" | "audio" | "other";

declare type UserFileDto = {
  fileId: string;
  fileName: string;
  url: string;
  typeId: string;
  fileTypeName: string;
  extensionId: string;
  fileExtensionName: string;
  size: number;
  createdAt: Date;
  ownerId: string;
  ownerFirstName: string;
  ownerLastName: string;
  ownerImageUrl: string;
};

declare type UserFileShared = {
  id: string;
  firstName: string;
  lastName: string;
  email: string;
  imageUrl: string | null;
  createdAt: Date;
};

declare type FileIdDto = {
  fileId: string;
};

interface FilesContextType {
  files: UserFileDto[] | null;
  setFiles: (files: UserFileDto[] | null) => void;
  notification: boolean;
  setNotification: (notification: boolean) => void;
}

interface FilesProviderProps {
  children: ReactNode;
}

// OTHER

interface Params {
  pageName: string;
}

declare interface ActionType {
  label: string;
  icon: string;
  value: string;
}

declare interface SearchParamProps {
  params?: Promise<SegmentParams>;
  searchParams?: Promise<{ [key: string]: string | string[] | undefined }>;
}

declare interface UploadFileProps {
  file: File;
  user: User;
}
declare interface GetFilesProps {
  types: FileType[];
  searchText?: string;
  sort?: string;
  limit?: number;
}
declare interface RenameFileProps {
  fileId: string;
  name: string;
  extension: string;
  path: string;
}
declare interface UpdateFileUsersProps {
  fileId: string;
  emails: string[];
  path: string;
}
declare interface DeleteFileProps {
  fileId: string;
  bucketFileId: string;
  path: string;
}

declare interface FileUploaderProps {
  ownerId: string;
  accountId: string;
  className?: string;
}

declare interface MobileNavigationProps {
  ownerId: string;
  accountId: string;
  fullName: string;
  avatar: string;
  email: string;
}
declare interface SidebarProps {
  fullName: string;
  avatar: string;
  email: string;
}

declare interface ThumbnailProps {
  type: string;
  extension: string;
  url: string;
  className?: string;
  imageClassName?: string;
}

declare interface ShareInputProps {
  file: Models.Document;
  onInputChange: (e: React.ChangeEvent<HTMLInputElement>) => void;
  onRemove: (email: string) => void;
}

declare type FileDeletionDto = {
  fileId: string;
  fileUrl: string;
  user: User;
};

declare type FileDeletionBackendDto = {
  fileId: string;
  fileUrl: string;
};

declare type FileRenameDto = {
  fileId: string;
  fileName: string;
  user: User;
};

declare type FileShareBackendDto = {
  fileId: string;
  emails: string[];
};

declare type FileShareDto = {
  fileId: string;
  emails: string[];
  user: User;
};

declare type FileRemoveShareBackendDto = {
  fileId: string;
  userEmailToRemove: string;
};

declare type FileRemoveShareDto = {
  fileId: string;
  userEmailToRemove: string;
  user: User;
};

declare type ActionProps = {
  file: UserFileDto;
  notifyParent: () => unknown;
};

declare type ImageChangeDto = {
  image: File;
  user: User;
};

declare type ImageDeleteDto = {
  imageUrl: string;
  user: User;
};

declare type NameChangeDto = {
  firstName: string;
  lastName: string;
  user: User;
};
