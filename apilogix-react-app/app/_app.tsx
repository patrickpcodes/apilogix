import { AppProps } from "next/app"; // Import AppProps
import "@/styles/globals.css";

export default function App({ Component, pageProps }: AppProps) {
  // Specify types
  return <Component {...pageProps} />;
}
