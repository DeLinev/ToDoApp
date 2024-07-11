import { configureStore } from '@reduxjs/toolkit'
import tasksReducer from './tasksSlice'
import categoriesReducer from './categoriesSlice'
import { createEpicMiddleware } from 'redux-observable'
import { rootEpic } from './epics'

const epicMiddleware = createEpicMiddleware()

const store = configureStore({
    reducer: {
        tasks: tasksReducer,
        categories: categoriesReducer
    },
    middleware: (getDefaultMiddleware) =>
        getDefaultMiddleware()
            .concat(epicMiddleware)
})

epicMiddleware.run(rootEpic)

export default store