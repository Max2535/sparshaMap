const requestWeatherForecastsType = 'REQUEST_WEATHER_FORECASTS';
const receiveWeatherForecastsType = 'RECEIVE_WEATHER_FORECASTS';
const initialState = { jsonData: [], isLoading: false };

export const actionCreators = {
    requestWeatherForecasts: startDateIndex => async (dispatch, getState) => {
        if (startDateIndex === getState().weatherForecasts.startDateIndex) {
            // Don't issue a duplicate request (we already have or are loading the requested data)
            return;
        }

        dispatch({ type: requestWeatherForecastsType, startDateIndex });

        try {
            const url = `api/Ads/SearchData?startDateIndex=${startDateIndex}`;
            const response = await fetch(url);
            const json = await response.json();
            console.log(json);
            dispatch({ type: receiveWeatherForecastsType, startDateIndex, json });
        } catch (error) {
            alert(error);
        }
    }
};

export const reducer = (state, action) => {
    state = state || initialState;

    if (action.type === requestWeatherForecastsType) {
        return {
            ...state,
            startDateIndex: action.startDateIndex,
            isLoading: true
        };
    }

    if (action.type === receiveWeatherForecastsType) {
        return {
            ...state,
            startDateIndex: action.startDateIndex,
            jsonData: action.json,
            isLoading: false
        };
    }

    return state;
};
