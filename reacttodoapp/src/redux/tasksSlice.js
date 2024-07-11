import { createSlice } from '@reduxjs/toolkit'

const tasksSlice = createSlice({
    name: 'tasks',
    initialState: [],
    reducers: {
        addTask: (state, action) => {
            state.push(action.payload)
        },
        completeTask: (state, action) => {
            const task = state.find(task => task.id === action.payload);
            if (task)
                task.isCompleted = true;
        },
        deleteTask: (state, action) => {
            return state.filter(task => task.id !== action.payload);
        },
        fetchTasksSuccess: (state, action) => {
            return action.payload
        }
    }
})

export const { addTask, completeTask, deleteTask } = tasksSlice.actions
export default tasksSlice.reducer