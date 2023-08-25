export function getSearchParam(searchParams: URLSearchParams, name: string): string | null {
    const newParams = new URLSearchParams();
    for (const [name, value] of searchParams) {
        newParams.append(name.toLowerCase(), value);
    }

    return newParams.get(name.toLowerCase())
}

export function initialTheme(): string | null {
    const theme = localStorage.getItem('theme')
    if (theme === null) {
        return ''
    }

    const html = document.querySelector('html');
    html?.classList.add(theme ?? '')

    return theme
}