import React from 'react'
import { useDispatch, useSelector } from 'react-redux'
import { createTask } from '../redux/actions'

export default function TaskForm() {
    const [title, setTitle] = React.useState('')
    const [description, setDescription] = React.useState('')
    const [dueDate, setDueDate] = React.useState('')
    const [selectedCategories, setSelectedCategories] = React.useState([])
    const dispatch = useDispatch()

    const categories = useSelector(state => state.categories)
    const categoriesElements = categories.map(category => {
        return (
            <div key={category.id}>
                <input
                    type="checkbox"
                    className="btn-check"
                    id={category.id}
                    name="categories"
                    value={category.id}
                    onChange={handleCategoryChange}
                    checked={selectedCategories.includes(category.id)}
                />
                <label
                    className="btn btn-outline-warning rounded-pill"
                    htmlFor={category.id}>
                    {category.name}
                </label>
            </div>
        )
    })

    function handleSubmit(e) {
        e.preventDefault()
        if (description) {
            const date = new Date(dueDate)
            const houersDiff = date.getHours() - date.getTimezoneOffset() / 60
            const minutesDiff = (date.getMinutes() - date.getTimezoneOffset()) % 60
            date.setHours(houersDiff)
            date.setMinutes(minutesDiff)
            dispatch(createTask({
                id: Date.now(),
                title,
                description,
                dueDate: date.toJSON(),
                creationDate: Date.now(),
                isCompleted: false,
                categoryIds: selectedCategories
            }))
            setTitle('')
            setDescription('')
            setDueDate('')
            setSelectedCategories([]);
        }
    }

    function handleCategoryChange(e) {
        const categoryId = parseInt(e.target.value)
        if (e.target.checked)
            setSelectedCategories([...selectedCategories, categoryId])
        else
            setSelectedCategories(selectedCategories.filter(id => id !== categoryId))
    }

    return (
        <>
            <h1>New Task: </h1>
            <form onSubmit={handleSubmit}>
                <div className="d-flex gap-2 align-items-end mb-2">
                    <div className="form-group">
                        <label className="control-label">Title
                        <input
                            type="text"
                            value={title}
                            onChange={(e) => setTitle(e.target.value)}
                            className="form-control"
                            />
                        </label>
                    </div>
                    <div className="form-group">
                        <label className="control-label">Description
                        <input
                            type="text"
                            value={description}
                            onChange={(e) => setDescription(e.target.value)}
                            className="form-control"
                            required
                            />
                        </label>
                    </div>
                    <div className="form-group">
                        <label className="control-label">Due Date
                        <input
                            type="datetime-local"
                            value={dueDate}
                            onChange={(e) => setDueDate(e.target.value)}
                            className="form-control"
                            />
                        </label>
                    </div>
                    <div className="form-group">
                        <input type="submit" value="Create" className="btn btn-primary" />
                    </div>
                </div>
                <div className="form-group d-flex gap-1 flex-wrap">
                    {categoriesElements}
                </div>
            </form>
        </>
    )
}