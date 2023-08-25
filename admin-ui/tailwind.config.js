const { fontFamily } = require("tailwindcss/defaultTheme")

module.exports = {
    darkMode: 'class',
    important: true,
    content: [
        './src/pages/**/*.tsx',
        './src/components/**/*.tsx',
        './src/layouts/**/*.tsx',
        "./node_modules/react-tailwindcss-datepicker/dist/index.esm.js"
    ],
    theme: {
        extend: {
            aria: {},
            keyframes: {},
            animation: {},
            boxShadow: {
                '2md': '0 4px 20px rgba(91,115,139,.18), 0 0 2px rgba(91,115,139,.16)'
            },
            colors: {
                background: "hsl(var(--background))",
                foreground: "hsl(var(--foreground))",
                primary: {
                    DEFAULT: "hsl(var(--primary))",
                    foreground: "hsl(var(--primary-foreground))",
                },
                secondary: {
                    DEFAULT: "hsl(var(--secondary))",
                    foreground: "hsl(var(--secondary-foreground))",
                },
                destructive: {
                    DEFAULT: "hsl(var(--destructive))",
                    foreground: "hsl(var(--destructive-foreground))",
                },
                muted: {
                    DEFAULT: "hsl(var(--muted))",
                    foreground: "hsl(var(--muted-foreground))",
                },
                accent: {
                    DEFAULT: "hsl(var(--accent))",
                    foreground: "hsl(var(--accent-foreground))",
                },
                popover: {
                    DEFAULT: "hsl(var(--popover))",
                    foreground: "hsl(var(--popover-foreground))",
                },
                card: {
                    DEFAULT: "hsl(var(--card))",
                    foreground: "hsl(var(--card-foreground))",
                },
            }
        },
    },
    plugins: [
        require('@tailwindcss/forms'),
        require('@headlessui/tailwindcss'),
        require("@thoughtbot/tailwindcss-aria-attributes"),
        require("tailwindcss-animate")
    ],
}
