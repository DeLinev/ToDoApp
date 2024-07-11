import React, { useEffect } from 'react'
import { useDispatch, useSelector } from 'react-redux'
import { fetchTasks, updateTask, deleteTask } from '../redux/actions'

export default function TaskList() {
    const dispatch = useDispatch()

    useEffect(() => {
        dispatch(fetchTasks());
    }, [dispatch]);

    const tasks = useSelector(state => state.tasks)

    const pendingTasks = tasks.filter(task => !task.isCompleted)
    const completedTasks = tasks.filter(task => task.isCompleted)

    const pendingTasksElements = pendingTasks.map(task => {
        const taskDueDate = new Date(task.dueDate)
        let dueDateSpan
        if (!task.dueDate)
            dueDateSpan = <span className="border rounded-pill p-1 mb-1 d-inline-block">No Due Date</span>
        else if (taskDueDate < Date.now())
            dueDateSpan = <span className="border rounded-pill p-1 mb-1 d-inline-block text-danger">Due {taskDueDate.toLocaleString()}</span>
        else
            dueDateSpan = <span className="border rounded-pill p-1 mb-1 d-inline-block">Due {taskDueDate.toLocaleString()}</span>

        let categoriesElements;
        if (task.categories)
            categoriesElements = task.categories.map(category =>
                <span key={category.id} className="border border-warning bg-opacity-25 bg-warning rounded-pill p-1 mb-1 d-inline-block">
                    {category.name}
                </span>)

        const taskCreationDate = new Date(task.creationDate)
        return (
            <div className="border rounded p-3 mb-3" key={task.id}>
                <h2>{task.title}</h2>
                <div className="d-flex gap-1">
                    {dueDateSpan}
                    {categoriesElements}
                </div>
                <section className="h5">{task.description}</section>
                <section className="fst-italic fw-light text-secondary mb-1">Created {taskCreationDate.toLocaleString()}</section>
                <button className="btn btn-success" onClick={() => dispatch(updateTask(task.id))}>Mark as completed</button>
            </div>
        )
    })

    const completedTasksElements = completedTasks.map(task => {
        let taskCreationDate = new Date(task.creationDate)

        let categoriesElements;
        if (task.categories)
            categoriesElements = task.categories.map(category =>
                <span key={category.id} className="border border-warning bg-opacity-25 bg-warning rounded-pill p-1 mb-1 d-inline-block">
                    {category.name}
                </span>)
        return (
            <div className="border rounded p-3 mb-3" key={task.id}>
                <h2>{task.title}</h2>
                <div className="d-flex gap-1">
                    {categoriesElements}
                </div>
                <section className="h5">{task.description}</section>
                <section className="fst-italic fw-light text-secondary mb-1">
                    Created {taskCreationDate.toDateString()}
                </section>
                <button className="btn btn-danger" onClick={() => dispatch(deleteTask(task.id))}>Delete</button>
            </div>
        )
    })

    return (
        <>
            <h1 className="text-danger mt-2">Pending tasks</h1>
            {pendingTasksElements}
            <h1 className="text-success">Comleted tasks</h1>
            {completedTasksElements}
        </>
    )
}