const trustedDomains = {
    default: [
        "http://localhost:5129",
        'http://localhost:5129/connect/revocat'
    ]
};

// Service worker will continue to give access token to the JavaScript client
// Ideal to hide refresh token from client JavaScript, but to retrieve access_token for some
// scenarios which require it. For example, to send it via websocket connection.
trustedDomains.config_show_access_token = { domains: ["http://localhost:5129"], showAccessToken: true };