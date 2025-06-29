"use client";

import * as React from "react";

import { Progress } from "./progress";

export default function LinearProgressWithLabelDemo({
  ...props
}: {
  progress: number;
}) {
  const [progress, setProgress] = React.useState(props.progress);

  React.useEffect(() => {
    const timer = setTimeout(() => setProgress(props.progress), 500);
    return () => clearTimeout(timer);
  }, []);

  return (
    <div className="text-white w-full flex items-center justify-center gap-3 pr-5 lg:w-1/2 lg:min-w-1/2">
      <Progress value={progress} className="w-[60%]" />
      <span className="text-sm font-bold">{progress}%</span>
    </div>
  );
}
