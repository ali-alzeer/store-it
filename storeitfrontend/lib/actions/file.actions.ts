"use server";

import { Config } from "../config";

export const uploadFile = async ({ file, user }: UploadFileProps) => {
  try {
    const formData = new FormData();

    formData.append("file", file);

    const uploadingResult = await fetch(`${Config.baseUrl}/api/file/upload`, {
      method: "POST",
      body: formData,
      headers: {
        Authorization: `Bearer ${user.jwt}`,
      },
    });

    const uploadingData = await uploadingResult.json();

    if (uploadingResult.status === 200) {
      return { file, error: null };
    } else {
      if (uploadingData.detail) {
        return {
          file: null,
          error: `${uploadingData.detail}`,
        };
      } else if (uploadingData.title) {
        return {
          file: null,
          error: `${uploadingData.title}`,
        };
      } else {
        return {
          file: null,
          error: `Something happened with code (${uploadingResult.status})`,
        };
      }
    }
  } catch {
    return {
      file: null,
      error: `Something went wrong`,
    };
  }
};

export const getAllFilesForUser = async (user: User) => {
  try {
    const getAllFilesForUserResult = await fetch(
      `${Config.baseUrl}/api/file/user-files`,
      {
        method: "GET",
        headers: {
          Authorization: `Bearer ${user.jwt}`,
        },
      }
    );

    const getAllFilesForUserData = await getAllFilesForUserResult.json();

    if (getAllFilesForUserResult.status === 200) {
      return { files: getAllFilesForUserData, error: null };
    } else {
      if (getAllFilesForUserData.detail) {
        return {
          files: null,
          error: `${getAllFilesForUserData.detail}`,
        };
      } else if (getAllFilesForUserData.title) {
        return {
          files: null,
          error: `${getAllFilesForUserData.title}`,
        };
      } else {
        return {
          files: null,
          error: `Something happened with code (${getAllFilesForUserResult.status})`,
        };
      }
    }
  } catch {
    return {
      files: null,
      error: `Something went wrong`,
    };
  }
};

export const getAllUsersForFile = async (user: User, fileId: string) => {
  try {
    const getAllUsersForFileResult = await fetch(
      `${Config.baseUrl}/api/file/file-users?fileId=${fileId}`,
      {
        method: "GET",
        headers: {
          Authorization: `Bearer ${user.jwt}`,
        },
      }
    );

    const getAllUsersForFileData = await getAllUsersForFileResult.json();

    if (getAllUsersForFileResult.status === 200) {
      return { users: getAllUsersForFileData, error: null };
    } else {
      if (getAllUsersForFileData.detail) {
        return {
          users: null,
          error: `${getAllUsersForFileData.detail}`,
        };
      } else if (getAllUsersForFileData.title) {
        return {
          users: null,
          error: `${getAllUsersForFileData.title}`,
        };
      } else {
        return {
          users: null,
          error: `Something happened with code (${getAllUsersForFileResult.status})`,
        };
      }
    }
  } catch {
    return {
      users: null,
      error: `Something went wrong`,
    };
  }
};

export const deleteFile = async (fileDeletionDto: FileDeletionDto) => {
  try {
    const fileDeletionBackendDto: FileDeletionBackendDto = {
      fileId: fileDeletionDto.fileId,
      fileUrl: fileDeletionDto.fileUrl,
    };

    const deleteFileResult = await fetch(`${Config.baseUrl}/api/file/delete`, {
      method: "POST",
      body: JSON.stringify(fileDeletionBackendDto),
      headers: {
        Authorization: `Bearer ${fileDeletionDto.user.jwt}`,
        "Content-Type": "application/json",
      },
    });

    const deleteData = await deleteFileResult.json();
    if (deleteFileResult.status === 200) {
      return { success: deleteData, error: null };
    } else {
      if (deleteData.detail) {
        return {
          success: null,
          error: `${deleteData.detail}`,
        };
      } else if (deleteData.title) {
        return {
          success: null,
          error: `${deleteData.title}`,
        };
      } else {
        return {
          success: null,
          error: `Something happened with code (${deleteFileResult.status})`,
        };
      }
    }
  } catch {
    return {
      success: null,
      error: `Something went wrong`,
    };
  }
};

export const renameFile = async (fileRenameDto: FileRenameDto) => {
  try {
    const renameFileResult = await fetch(`${Config.baseUrl}/api/file/rename`, {
      method: "POST",
      body: JSON.stringify(fileRenameDto),
      headers: {
        Authorization: `Bearer ${fileRenameDto.user.jwt}`,
        "Content-Type": "application/json",
      },
    });

    const renameData = await renameFileResult.json();
    if (renameFileResult.status === 200) {
      return { success: renameData, error: null };
    } else {
      if (renameData.detail) {
        return {
          success: null,
          error: `${renameData.detail}`,
        };
      } else if (renameData.title) {
        return {
          success: null,
          error: `${renameData.title}`,
        };
      } else {
        return {
          success: null,
          error: `Something happened with code (${renameFileResult.status})`,
        };
      }
    }
  } catch {
    return {
      success: null,
      error: `Something went wrong`,
    };
  }
};

export const shareFile = async (fileShareDto: FileShareDto) => {
  try {
    const fileShareBackendDto: FileShareBackendDto = {
      emails: fileShareDto.emails,
      fileId: fileShareDto.fileId,
    };

    const shareFileResult = await fetch(`${Config.baseUrl}/api/file/share`, {
      method: "POST",
      body: JSON.stringify(fileShareBackendDto),
      headers: {
        Authorization: `Bearer ${fileShareDto.user.jwt}`,
        "Content-Type": "application/json",
      },
    });

    const shareData = await shareFileResult.json();
    if (shareFileResult.status === 200) {
      return { success: shareData, error: null };
    } else {
      if (shareData.detail) {
        return {
          success: null,
          error: `${shareData.detail}`,
        };
      } else if (shareData.title) {
        return {
          success: null,
          error: `${shareData.title}`,
        };
      } else {
        return {
          success: null,
          error: `Something happened with code (${shareFileResult.status})`,
        };
      }
    }
  } catch {
    return {
      success: null,
      error: `Something went wrong`,
    };
  }
};

export const removeUserForSharedFile = async (
  fileRemoveShareDto: FileRemoveShareDto
) => {
  try {
    const fileRemoveShareBackendDto: FileRemoveShareBackendDto = {
      fileId: fileRemoveShareDto.fileId,
      userEmailToRemove: fileRemoveShareDto.userEmailToRemove,
    };

    const removeShareFileResult = await fetch(
      `${Config.baseUrl}/api/file/share-remove`,
      {
        method: "POST",
        body: JSON.stringify(fileRemoveShareBackendDto),
        headers: {
          Authorization: `Bearer ${fileRemoveShareDto.user.jwt}`,
          "Content-Type": "application/json",
        },
      }
    );

    const removeShareData = await removeShareFileResult.json();
    if (removeShareFileResult.status === 200) {
      return { success: removeShareData, error: null };
    } else {
      if (removeShareData.detail) {
        return {
          success: null,
          error: `${removeShareData.detail}`,
        };
      } else if (removeShareData.title) {
        return {
          success: null,
          error: `${removeShareData.title}`,
        };
      } else {
        return {
          success: null,
          error: `Something happened with code (${removeShareFileResult.status})`,
        };
      }
    }
  } catch {
    return {
      success: null,
      error: `Something went wrong`,
    };
  }
};
