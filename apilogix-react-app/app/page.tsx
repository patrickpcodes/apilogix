import Link from "next/link"; // Import the Link component

export default function Home() {
  return (
    <div className="min-h-screen flex items-center justify-center">
      <h1 className="text-2xl font-bold">Welcome to My Homepage</h1>
      <p>
        <Link href="/WeatherForecast">Go to Weather Forecast in AWS</Link>
      </p>
    </div>
  );
}
