"use client";
import { useState } from "react"; // Import useState for managing state

export default function Home() {
  const [apiResponse, setApiResponse] = useState(null); // State to store API response

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
      console.log(data);
      setApiResponse(data); // Update state with the fetched data
      return data;
    } catch (error) {
      console.error("There was an error:", error);
    }
  }

  return (
    <div className="min-h-screen flex items-center justify-center flex-col">
      <h1 className="text-2xl font-bold">Welcome to My Homepage</h1>
      <button
        onClick={fetchFromApi}
        className="mt-4 p-2 bg-blue-500 text-white rounded"
      >
        Fetch Data
      </button>
      {apiResponse && (
        <pre className="mt-4">{JSON.stringify(apiResponse, null, 2)}</pre>
      )}{" "}
      {/* Display API response */}
    </div>
  );
}
