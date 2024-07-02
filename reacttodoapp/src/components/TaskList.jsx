import React from 'react'
import { useDispatch, useSelector } from 'react-redux'
import { completeTask, deleteTask } from '../redux/tasksSlice'

export default function TaskList() {
    const dispatch = useDispatch()

    const tasks = useSelector(state => state.tasks)
    const categories = useSelector(state => state.categories)

    const pendingTasks = tasks.filter(task => !task.completed)
    const completedTasks = tasks.filter(task => task.completed)

    const pendingTasksElements = pendingTasks.map(task => {
        const taskDueDate = new Date(task.dueDate)
        let dueDateSpan
        if (isNaN(taskDueDate))
            dueDateSpan = <span className="border rounded-pill p-1 mb-1 d-inline-block">No Due Date</span>
        else if (taskDueDate < Date.now())
            dueDateSpan = <span className="border rounded-pill p-1 mb-1 d-inline-block text-danger">Due {taskDueDate.toLocaleString()}</span>
        else
            dueDateSpan = <span className="border rounded-pill p-1 mb-1 d-inline-block">Due {taskDueDate.toLocaleString()}</span>

        const taskCreationDate = new Date(task.id)
        return (
            <div className="border rounded p-3 mb-3" key={task.id}>
                <h2>{task.title}</h2>
                <div className="d-flex gap-1">
                    {dueDateSpan}
                    {getCategoriesElements(task.categories)}
                </div>
                <section className="h5">{task.description}</section>
                <section className="fst-italic fw-light text-secondary mb-1">Created {taskCreationDate.toLocaleString()}</section>
                <button className="btn btn-success" onClick={() => dispatch(completeTask(task.id))}>Mark as completed</button>
            </div>
        )
    })

    const completedTasksElements = completedTasks.map(task => {
        let taskCreationDate = new Date(task.id)

        return (
            <div className="border rounded p-3 mb-3" key={task.id}>
                <h2>{task.title}</h2>
                <div className="d-flex gap-1">
                    {getCategoriesElements(task.categories)}
                </div>
                <section className="h5">{task.description}</section>
                <section className="fst-italic fw-light text-secondary mb-1">
                    Created {taskCreationDate.toDateString()}
                </section>
                <button className="btn btn-danger" onClick={() => dispatch(deleteTask(task.id))}>Delete</button>
            </div>
        )
    })

    function getCategoryNameById(id) {
        const category = categories.find(category => category.id === id)
        return category ? category.name : ''
    }

    function getCategoriesElements(categoriesIds) {
        return categoriesIds.map(categoryId =>
            <span className="border border-warning bg-opacity-25 bg-warning rounded-pill p-1 mb-1 d-inline-block">
                {getCategoryNameById(categoryId)}
            </span>
        )
    }

    return (
        <>
            <h1 className="text-danger mt-2">Pending tasks</h1>
            {pendingTasksElements}
            <h1 className="text-success">Comleted tasks</h1>
            {completedTasksElements}
        </>
    )
}