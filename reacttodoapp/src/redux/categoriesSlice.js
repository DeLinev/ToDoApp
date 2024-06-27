import { createSlice } from '@reduxjs/toolkit'

const categoriesSlice = createSlice({
    name: 'categories',
    initialState: [
        {
            id: 1,
            name: "Work"
        },
        {
            id: 2,
            name: "Study"
        },
        {
            id: 3,
            name: "Shopping"
        },
        {
            id: 4,
            name: "Math"
        },
        {
            id: 5,
            name: "Programming"
        },
        {
            id: 6,
            name: "Important"
        },
        {
            id: 1002,
            name: "Films"
        },
        {
            id: 1003,
            name: "Books"
        }
    ]
})

export default categoriesSlice.reducer