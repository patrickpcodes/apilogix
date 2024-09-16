import type { Config } from "tailwindcss";

const config: Config = {
  darkMode: 'class', // Enable dark mode using the 'class' strategy
  content: [
    "./app/**/*.{js,ts,jsx,tsx,mdx}",
    "./pages/**/*.{js,ts,jsx,tsx,mdx}",
    "./components/**/*.{js,ts,jsx,tsx,mdx}",
  ],
  theme: {
    extend: {
      colors: {
        // background: '#1a1a1a',  // Test with a fixed color
        // foreground: '#f5f5f5',
        background: 'var(--background)',
        foreground: 'var(--foreground)',
      },
    },
  },
  plugins: [require("tailwindcss-animate")],
};

export default config;
