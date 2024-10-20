/** @type {import('tailwindcss').Config} */
export default {
  content: [
    "./index.html",
    "./src/**/*.{vue,js,ts,jsx,tsx}",
  ],
  theme: {
    extend: {
      backgroundImage: {
        'icon-discussion': "url('/path/to/icon-home.svg')",
        'icon-user': "url('/path/to/icon-user.svg')",
      },
    },
  },
  plugins: [],
}
