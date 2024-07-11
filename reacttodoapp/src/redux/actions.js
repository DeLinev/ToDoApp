export const FETCH_TASKS = 'FETCH_TASKS'
export const FETCH_TASKS_SUCCESS = 'tasks/fetchTasksSuccess'
export const CREATE_TASK = 'CREATE_TASK'
export const CREATE_TASK_SUCCESS = 'tasks/addTask'
export const UPDATE_TASK = 'UPDATE_TASK'
export const UPDATE_TASK_SUCCESS = 'tasks/completeTask'
export const DELETE_TASK = 'DELETE_TASK'
export const DELETE_TASK_SUCCESS = 'tasks/deleteTask'

export const fetchTasks = () => ({ type: FETCH_TASKS })
export const fetchTasksSuccess = (tasks) => ({ type: FETCH_TASKS_SUCCESS, payload: tasks })
export const createTask = (task) => ({ type: CREATE_TASK, payload: task })
export const createTaskSuccess = (task) => ({ type: CREATE_TASK_SUCCESS, payload: task })
export const updateTask = (id) => ({ type: UPDATE_TASK, payload: id })
export const updateTaskSuccess = (id) => ({ type: UPDATE_TASK_SUCCESS, payload: id })
export const deleteTask = (id) => ({ type: DELETE_TASK, payload: id })
export const deleteTaskSuccess = (id) => ({ type: DELETE_TASK_SUCCESS, payload: id })