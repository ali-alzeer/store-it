import { renderWithFilesContext } from "../test-utils";
import { screen, waitForElementToBeRemoved } from "@testing-library/react";
import HomePage from "../components/HomePage";
import "@testing-library/jest-dom";

// Mock useParams
jest.mock("next/navigation", () => ({
  useParams: jest.fn().mockReturnValue({ pageName: "" }),
}));
import { useParams } from "next/navigation";

// Mock all utils
jest.mock("../lib/utils", () => {
  const actual = jest.requireActual("../lib/utils");
  return {
    ...actual,
    filterFilesAccordingToParams: jest.fn((_, files) => files),
  };
});

// Stub out child components
jest.mock(
  "../components/Sort",
  () =>
    function Sort() {
      return <div data-testid="sort">Sort Component</div>;
    }
);
jest.mock(
  "../components/Card",
  () =>
    function Card({ file }: { file: UserFileDto }) {
      return <div data-testid="card">Card Component {file.fileName}</div>;
    }
);
jest.mock(
  "../components/ui/progress-11",
  () =>
    function Progress11() {
      return <div>CircleProgress</div>;
    }
);
jest.mock(
  "../components/ui/progress-02",
  () =>
    function Progress02() {
      return <div>LinearProgress</div>;
    }
);

describe("HomePage", () => {
  beforeEach(() => {
    jest.clearAllMocks();
  });
  test("Show loading, then show HomPage with 2 file cards and Dashboard title", async () => {
    (useParams as jest.Mock).mockReturnValue({ pageName: "" });

    renderWithFilesContext(<HomePage />);

    const loader = await screen.findByTestId("loading");
    expect(loader).toBeInTheDocument();

    await waitForElementToBeRemoved(() => screen.queryByTestId("loading"));

    expect(await screen.findByTestId("home")).toBeInTheDocument();
    expect(await screen.findByText("Dashboard")).toBeInTheDocument();
    expect(await screen.findAllByTestId("card")).toHaveLength(2);
  });
  test("Show Media when pageName is media", async () => {
    (useParams as jest.Mock).mockReturnValue({ pageName: "media" });

    renderWithFilesContext(<HomePage />);

    const loader = await screen.findByTestId("loading");
    expect(loader).toBeInTheDocument();

    await waitForElementToBeRemoved(() => screen.queryByTestId("loading"));

    expect(await screen.findByTestId("home")).toBeInTheDocument();
    expect(await screen.findByText("Media")).toBeInTheDocument();
  });
});
