const EXTERNAL_HOST = "https://localhost:5001";

export const SIGNALR = {
    WEST: `${EXTERNAL_HOST}/health-check/west`,
    EAST: `${EXTERNAL_HOST}/health-check/east`,
    SOUTH: `${EXTERNAL_HOST}/health-check/south`,
};

export const API = {
    EXTERNAL_HOST,
    WEST: `${EXTERNAL_HOST}/api/health-check/west`,
    EAST: `${EXTERNAL_HOST}/api/health-check/east`,
    SOUTH: `${EXTERNAL_HOST}/api/health-check/south`,
};
