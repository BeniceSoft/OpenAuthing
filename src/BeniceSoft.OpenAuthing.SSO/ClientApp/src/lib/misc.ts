export function getSearchParam(searchParams: URLSearchParams, name: string): string | null {
    const newParams = new URLSearchParams();
    for (const [name, value] of searchParams) {
        newParams.append(name.toLowerCase(), value);
    }

    return newParams.get(name.toLowerCase())
}

export function syncTheme() {
    const theme = localStorage.getItem('hs_theme')
    if (theme === null) {
        return ''
    }

    const html = document.querySelector('html');

    if (html?.classList.contains(theme)) return
    html?.classList.add(theme ?? '')
}