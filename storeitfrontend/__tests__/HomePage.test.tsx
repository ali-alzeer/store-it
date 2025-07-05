import { screen, waitForElementToBeRemoved } from "@testing-library/react";
import HomePage from "../components/HomePage";
import "@testing-library/jest-dom";

// Mock next/navigation
jest.mock("next/navigation", () => ({
  useParams: jest.fn(() => {
    return { pageName: "" };
  }),
}));

// Mock constants
jest.mock("../constants/index", () => ({
  MAX_CAPACITY_SIZE_FOR_USER: 104857600,
}));

// Mock all utils
jest.mock("../lib/utils", () => ({
  calculatePercentage: jest.fn(),
  calculateTotalFileSize: jest.fn(),
  convertFileSize: jest.fn(),
  filterFilesAccordingToParams: jest.fn((_, files) => files),
  isValidPageName: jest.fn(),
  titleCase: jest.fn(),
  saveLoggedInUserToLocalStorage: jest.fn(),
}));
import { FilesRender } from "../test-utils";

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
  test("Show loading, then show HomPage with 2 file cards and Dashboard title", async () => {
    FilesRender(<HomePage />);

    const loader = await screen.findByTestId("loading");
    expect(loader).toBeInTheDocument();

    await waitForElementToBeRemoved(() => screen.queryByTestId("loading"));

    expect(await screen.findByTestId("home")).toBeInTheDocument();
    expect(await screen.findByText("Dashboard")).toBeInTheDocument();
    expect(await screen.findAllByTestId("card")).toHaveLength(2);
  });
});
