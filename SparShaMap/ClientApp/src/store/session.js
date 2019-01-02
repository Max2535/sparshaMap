const loginType = 'LOG-IN';
const logoutType = 'LOG-OUT';//TODO: gettoken
const initialState = { token: "eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiIwMDciLCJ1bmlxdWVfbmFtZSI6InN1cHBjaGFpIiwibmJmIjoxNTQ1MjA2MjY3LCJleHAiOjE1NDUyOTI2NjcsImlhdCI6MTU0NTIwNjI2N30.ax7YXaB09DSz6qWGnhYtWVtQAJb3TWtN1utbIIFSukQ3tZd7N3sRJhSVPD4jYeOewwzSy1qiPvW95f7iwYgTKg" };

export const actionCreators = {
    login: () => ({ type: loginType }),
    logout: () => ({ type: logoutType })
};

export const reducer = (state, action) => {
    state = state || initialState;
    if (action.type === loginType) {
        return { ...state, token: state.token };
    }

    if (action.type === logoutType) {
        return { ...state, token:""};
    }

    return state;
};
