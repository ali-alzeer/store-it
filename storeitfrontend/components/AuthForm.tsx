"use client";

import { z } from "zod";
import { zodResolver } from "@hookform/resolvers/zod";
import { useForm } from "react-hook-form";

import { Button } from "../components/ui/button";
import {
  Form,
  FormControl,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from "../components/ui/form";
import { Input } from "../components/ui/input";
import { useContext, useState } from "react";
import Image from "next/image";
import Link from "next/link";
import { createAccount, signInUser } from "../lib/actions/user.actions";
import { AuthContext } from "../contexts/AuthContext";
import { saveLoggedInUserToLocalStorage } from "../lib/utils";
import { redirect } from "next/navigation";

type FormType = "sign-in" | "sign-up";

const authFormSchema = (formType: FormType) => {
  return z.object({
    email: z.string().email(),
    password: z.string().min(6).max(50),
    firstName:
      formType === "sign-up"
        ? z.string().min(2).max(50)
        : z.string().optional(),
    lastName:
      formType === "sign-up"
        ? z.string().min(2).max(50)
        : z.string().optional(),
  });
};

const AuthForm = ({ type }: { type: FormType }) => {
  const { user, setUser } = useContext(AuthContext);

  const [isLoading, setIsLoading] = useState(false);
  const [errorMessage, setErrorMessage] = useState("");

  const [isPasswordHidden, setIsPasswordHidden] = useState(true);

  const formSchema = authFormSchema(type);
  const form = useForm<z.infer<typeof formSchema>>({
    resolver: zodResolver(formSchema),
    defaultValues: {
      firstName: "",
      lastName: "",
      password: "",
      email: "",
    },
  });

  const onSubmit = async (values: z.infer<typeof formSchema>) => {
    setIsLoading(true);
    setErrorMessage("");

    const { user, error } =
      type === "sign-up"
        ? await createAccount({
            firstName: values.firstName || "",
            lastName: values.lastName || "",
            password: values.password,
            email: values.email,
          })
        : await signInUser({
            password: values.password,
            email: values.email,
          });

    if (user !== null && user !== undefined) {
      setIsLoading(false);
      setErrorMessage("");
      saveLoggedInUserToLocalStorage(user);
      setUser(user);
      redirect("/");
    } else {
      setIsLoading(false);
      setErrorMessage(error);
      saveLoggedInUserToLocalStorage(null);
      setUser(null);
    }
  };

  if (user !== undefined && user !== null) {
    return redirect("/");
  } else {
    return (
      <>
        <Form {...form}>
          <form onSubmit={form.handleSubmit(onSubmit)} className="auth-form">
            <h1 className="form-title">
              {type === "sign-in" ? "Sign In" : "Sign Up"}
            </h1>
            {type === "sign-up" && (
              <>
                <FormField
                  control={form.control}
                  name="firstName"
                  render={({ field }) => (
                    <FormItem>
                      <div className="shad-form-item">
                        <FormLabel className="shad-form-label">
                          First Name
                        </FormLabel>

                        <FormControl>
                          <Input
                            placeholder="Enter your first name"
                            className="px-1 shad-input"
                            {...field}
                          />
                        </FormControl>
                      </div>

                      <FormMessage className="shad-form-message" />
                    </FormItem>
                  )}
                />

                <FormField
                  control={form.control}
                  name="lastName"
                  render={({ field }) => (
                    <FormItem>
                      <div className="shad-form-item">
                        <FormLabel className="shad-form-label">
                          Last Name
                        </FormLabel>

                        <FormControl>
                          <Input
                            placeholder="Enter your last name"
                            className="px-1 shad-input"
                            {...field}
                          />
                        </FormControl>
                      </div>

                      <FormMessage className="shad-form-message" />
                    </FormItem>
                  )}
                />
              </>
            )}

            <FormField
              control={form.control}
              name="email"
              render={({ field }) => (
                <FormItem>
                  <div className="shad-form-item">
                    <FormLabel className="shad-form-label">Email</FormLabel>

                    <FormControl>
                      <Input
                        placeholder="Enter your email"
                        className="px-1 shad-input"
                        {...field}
                      />
                    </FormControl>
                  </div>

                  <FormMessage className="shad-form-message" />
                </FormItem>
              )}
            />

            <FormField
              control={form.control}
              name="password"
              render={({ field }) => (
                <FormItem>
                  <div className="shad-form-item">
                    <FormLabel className="shad-form-label">Password</FormLabel>

                    <FormControl>
                      <div>
                        <Input
                          type={isPasswordHidden ? "password" : "text"}
                          placeholder="Enter your password"
                          className="px-1 shad-input"
                          {...field}
                        />
                        <div
                          onClick={() => setIsPasswordHidden(!isPasswordHidden)}
                          className="cursor-pointer text-center mb-7 rounded-md p-1 text-xs bg-gray-200 text-gray-500"
                        >
                          {isPasswordHidden ? "Show" : "Hide"}
                        </div>
                      </div>
                    </FormControl>
                  </div>

                  <FormMessage className="shad-form-message" />
                </FormItem>
              )}
            />

            <Button
              type="submit"
              className="form-submit-button"
              disabled={isLoading}
            >
              {type === "sign-in" ? "Sign In" : "Sign Up"}

              {isLoading && (
                <Image
                  data-testid="loader"
                  src="/assets/icons/loader.svg"
                  alt="loader"
                  width={24}
                  height={24}
                  className="ml-2 animate-spin"
                />
              )}
            </Button>

            {errorMessage !== "" ? (
              <p data-testid="error" className="error-message">
                *{errorMessage}
              </p>
            ) : null}

            <div className="body-2 flex justify-center">
              <p className="text-light-100">
                {type === "sign-in"
                  ? "Don't have an account?"
                  : "Already have an account?"}
              </p>
              <Link
                href={type === "sign-in" ? "/sign-up" : "/sign-in"}
                className="ml-1 font-medium text-brand"
              >
                {" "}
                {type === "sign-in" ? "Sign Up" : "Sign In"}
              </Link>
            </div>
          </form>
        </Form>
      </>
    );
  }
};

export default AuthForm;
