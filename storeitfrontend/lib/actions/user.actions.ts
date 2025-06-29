"use server";

import { Config } from "../config";
import { parseStringify } from "../utils";

export const createAccount = async (signUpDto: SignUpDto) => {
  try {
    const signUpResult = await fetch(`${Config.baseUrl}/api/account/sign-up`, {
      method: "POST",
      body: JSON.stringify(signUpDto),
      headers: {
        "Content-Type": "application/json",
      },
    });
    const signUpData = await signUpResult.json();

    if (signUpResult.status === 200) {
      // If user was created successfully, sign in directly after sign up
      const signInDto: SignInDto = {
        email: signUpDto.email,
        password: signUpDto.password,
      };
      return await signInUser(signInDto, true);
    } else {
      if (signUpData.detail) {
        return { user: null, error: signUpData.detail };
      } else if (signUpData.title) {
        return { user: null, error: signUpData.title };
      } else {
        return {
          user: null,
          error: `Something went wrong (${signUpResult.status})`,
        };
      }
    }
  } catch {
    return {
      user: null,
      error: `Something went wrong`,
    };
  }
};

export const signInUser = async (signInDto: SignInDto, afterSignUp = false) => {
  try {
    const signInResult = await fetch(`${Config.baseUrl}/api/account/sign-in`, {
      method: "POST",
      body: JSON.stringify(signInDto),
      headers: {
        "Content-Type": "application/json",
      },
    });

    const signInData = await signInResult.json();

    if (signInResult.status === 200) {
      const newUser = parseStringify(signInData);
      return { user: newUser, error: "" };
    } else {
      if (signInData.detail) {
        return {
          user: null,
          error: afterSignUp
            ? `Account was created successfully but something happened during signing in with message : ${signInData.detail}`
            : `${signInData.detail}`,
        };
      } else if (signInData.title) {
        return {
          user: null,
          error: afterSignUp
            ? `Account was created successfully but something happened during signing in with message : ${signInData.title}`
            : `${signInData.title}`,
        };
      } else {
        return {
          user: null,
          error: afterSignUp
            ? `Account was created successfully but something happened during signing in with code : ${signInResult.status}`
            : `Something happened with code (${signInResult.status})`,
        };
      }
    }
  } catch {
    return {
      user: null,
      error: `Something went wrong`,
    };
  }
};

export const validateJwtToken = async (jwtDto: JwtDto) => {
  try {
    const tokenValidationResult = await fetch(
      `${Config.baseUrl}/api/account/validate-jwt`,
      {
        method: "POST",
        body: JSON.stringify(jwtDto),
        headers: {
          "Content-Type": "application/json",
        },
      }
    );

    const tokenValidationObject: TokenValidation =
      await tokenValidationResult.json();
    if (tokenValidationObject.isValid) {
      return true;
    } else {
      return false;
    }
  } catch {
    return false;
  }
};

export const changeUserImage = async (imageChangeDto: ImageChangeDto) => {
  try {
    const imageFormData = new FormData();
    imageFormData.append("image", imageChangeDto.image);

    const imageChangeResult = await fetch(
      `${Config.baseUrl}/api/account/image-change`,
      {
        method: "POST",
        body: imageFormData,
        headers: {
          Authorization: `Bearer ${imageChangeDto.user.jwt}`,
        },
      }
    );

    const imageChangeData = await imageChangeResult.json();

    if (imageChangeResult.status === 200) {
      const newUser = parseStringify(imageChangeData);
      return { user: newUser, error: null };
    } else {
      if (imageChangeData.detail) {
        return {
          user: null,
          error: `${imageChangeData.detail}`,
        };
      } else if (imageChangeData.title) {
        return {
          user: null,
          error: `${imageChangeData.title}`,
        };
      } else {
        return {
          user: null,
          error: `Something happened with code (${imageChangeResult.status})`,
        };
      }
    }
  } catch {
    return {
      user: null,
      error: `Something went wrong`,
    };
  }
};

export const deleteUserImage = async (imageDeleteDto: ImageDeleteDto) => {
  try {
    const imageDeleteResult = await fetch(
      `${Config.baseUrl}/api/account/image-delete`,
      {
        method: "POST",
        body: JSON.stringify({ imageUrl: imageDeleteDto.imageUrl }),
        headers: {
          Authorization: `Bearer ${imageDeleteDto.user.jwt}`,
          "Content-Type": "application/json",
        },
      }
    );

    const imageDeleteData = await imageDeleteResult.json();

    if (imageDeleteResult.status === 200) {
      const newUser = parseStringify(imageDeleteData);
      return { user: newUser, error: null };
    } else {
      if (imageDeleteData.detail) {
        return {
          user: null,
          error: `${imageDeleteData.detail}`,
        };
      } else if (imageDeleteData.title) {
        return {
          user: null,
          error: `${imageDeleteData.title}`,
        };
      } else {
        return {
          user: null,
          error: `Something happened with code (${imageDeleteResult.status})`,
        };
      }
    }
  } catch {
    return {
      user: null,
      error: `Something went wrong`,
    };
  }
};

export const changeUserName = async (nameChangeDto: NameChangeDto) => {
  try {
    const nameChangeResult = await fetch(
      `${Config.baseUrl}/api/account/name-change`,
      {
        method: "POST",
        body: JSON.stringify({
          firstName: nameChangeDto.firstName,
          lastName: nameChangeDto.lastName,
        }),
        headers: {
          Authorization: `Bearer ${nameChangeDto.user.jwt}`,
          "Content-Type": "application/json",
        },
      }
    );

    const nameChangeData = await nameChangeResult.json();

    if (nameChangeResult.status === 200) {
      const newUser = parseStringify(nameChangeData);
      return { user: newUser, error: null };
    } else {
      if (nameChangeData.detail) {
        return {
          user: null,
          error: `${nameChangeData.detail}`,
        };
      } else if (nameChangeData.title) {
        return {
          user: null,
          error: `${nameChangeData.title}`,
        };
      } else {
        return {
          user: null,
          error: `Something happened with code (${nameChangeResult.status})`,
        };
      }
    }
  } catch {
    return {
      user: null,
      error: `Something went wrong`,
    };
  }
};

export const deleteUserAccount = async (user: User) => {
  try {
    const accountDeleteResult = await fetch(
      `${Config.baseUrl}/api/account/account-delete`,
      {
        method: "DELETE",
        headers: {
          Authorization: `Bearer ${user.jwt}`,
          "Content-Type": "application/json",
        },
      }
    );

    const accountDeleteData = await accountDeleteResult.json();

    if (accountDeleteResult.status === 200) {
      return { success: true, error: null };
    } else {
      if (accountDeleteData.detail) {
        return {
          success: null,
          error: `${accountDeleteData.detail}`,
        };
      } else if (accountDeleteData.title) {
        return {
          success: null,
          error: `${accountDeleteData.title}`,
        };
      } else {
        return {
          success: null,
          error: `Something happened with code (${accountDeleteResult.status})`,
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
