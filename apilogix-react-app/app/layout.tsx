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

export const AppLayout = ({ children }: { children: React.ReactNode }) => {
  return <ClientLayout>{children}</ClientLayout>;
};

// Main layout component
export default function Layout({ children }: { children: React.ReactNode }) {
  return (
    <html lang="en">
      <body>
        <AppLayout>{children}</AppLayout>
      </body>
    </html>
  );
}
