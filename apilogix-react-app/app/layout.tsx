"use client";

import React from "react"; // Import React
import "@/styles/globals.css";

// Client component for the layout
const ClientLayout = ({ children }: { children: React.ReactNode }) => {
  return (
    <div className="flex-1 flex flex-col h-screen">
      <main className="flex-1 overflow-y-auto">{children}</main>
    </div>
  );
};

// Main layout component
export default function Layout({ children }: { children: React.ReactNode }) {
  return (
    <html lang="en">
      <body>
        <ClientLayout>{children}</ClientLayout>
      </body>
    </html>
  );
}
