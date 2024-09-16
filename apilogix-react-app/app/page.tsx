"use client";

import React, { useState } from "react";
import { Button } from "@/components/ui/button"; // Import the Button component
import { AppToolbarComponent } from "@/components/app-toolbar"; // Import the AppToolbarComponent

export default function Home() {
  const [apiResponse, setApiResponse] = useState(null); // State to hold API response

  async function fetchFromApi() {
    try {
      const response = await fetch(
        `${process.env.NEXT_PUBLIC_API_URL}/WeatherForecast`,
        {
          method: "GET",
          headers: {
            "Content-Type": "application/json",
          },
        }
      );

      if (!response.ok) {
        throw new Error("Network response was not ok");
      }

      const data = await response.json();
      setApiResponse(data); // Set the API response to state
    } catch (error) {
      console.error("There was an error:", error);
    }
  }

  return (
    <div
      className={`flex-col items-center justify-center min-h-screen bg-background text-foreground`}
    >
      <AppToolbarComponent /> {/* Add the AppToolbarComponent here */}
      <br />
      <h1 className="text-4xl font-bold">
        Welcome To ApiLogix: App hosted on AWS
      </h1>
      <br />
      <Button onClick={fetchFromApi} className="mt-4">
        Fetch Weather Data
      </Button>{" "}
      {/* Button to fetch data */}
      {apiResponse && (
        <div className="mt-4 bg-background text-foreground">
          <h2 className="text-2xl font-semibold">API Response:</h2>
          <p>(apiResponse && {JSON.stringify(apiResponse, null, 2)})</p>
        </div>
      )}
    </div>
  );
}
