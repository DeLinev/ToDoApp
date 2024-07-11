import { ofType, combineEpics } from 'redux-observable'
import { from } from 'rxjs'
import { mergeMap } from 'rxjs/operators'
import {
    FETCH_TASKS,
    fetchTasksSuccess,
    CREATE_TASK,
    createTaskSuccess,
    UPDATE_TASK,
    updateTaskSuccess,
    DELETE_TASK,
    deleteTaskSuccess,
} from '../redux/actions';

const GET_TASKS_QUERY = `
  query {
      tasks {
        id,
        title,
        description,
        creationDate,
        dueDate,
        isCompleted,
        categories {
          id,
          name
        }
      }
    }`

const CREATE_TASK_MUTATION = `
    mutation (
      $title: String,
      $description: String!,
      $dueDate: DateTime,
      $isCompleted: Boolean!,
      $categoryIds: [Int]!
    ) {
      createTask(
        task: {
          title: $title,
          description: $description,
          dueDate: $dueDate,
          isCompleted: $isCompleted
        },
        categoryIds: $categoryIds
      ) {
        id,
        title,
        description,
        creationDate,
        dueDate,
        isCompleted,
        categories {
            id,
            name
        }
      }
    }`

const UPDATE_TASK_MUTATION = `
    mutation ($id: Int!) {
      updateTask(id: $id){
         id,
         title,
         description,
         creationDate,
         dueDate,
         isCompleted,
         categories {
             id,
             name
         }
       }
     }`

const DELETE_TASK_MUTATION = `
    mutation ($id: Int!) {
      deleteTask(id: $id){
         id,
         title,
         description,
         creationDate,
         dueDate,
         isCompleted,
         categories {
            id,
            name
        }
      }
    }`

const fetchTasksEpic = (action$) =>
    action$.pipe(
        ofType(FETCH_TASKS),
        mergeMap(() =>
            from(
                fetch('https://localhost:7047/graphql', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json',
                    },
                    body: JSON.stringify({ query: GET_TASKS_QUERY }),
                })
                    .then((response) => response.json())
                    .then((result) => result.data.tasks)
            ).pipe(
                mergeMap((tasks) => [fetchTasksSuccess(tasks)])
            )
        )
    )

const createTaskEpic = (action$) =>
    action$.pipe(
        ofType(CREATE_TASK),
        mergeMap((action) =>
            from(
                fetch('https://localhost:7047/graphql', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json',
                    },
                    body: JSON.stringify({
                        query: CREATE_TASK_MUTATION,
                        variables: { ...action.payload },
                    })
                })
                    .then((response) => response.json())
                    .then((result) => result.data.createTask)
            ).pipe(
                mergeMap((task) => [createTaskSuccess(task)])
            )
        )
    )

const updateTaskEpic = (action$) =>
    action$.pipe(
        ofType(UPDATE_TASK),
        mergeMap((action) =>
            from(
                fetch('https://localhost:7047/graphql', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json',
                    },
                    body: JSON.stringify({
                        query: UPDATE_TASK_MUTATION,
                        variables: { id: action.payload },
                    })
                })
                    .then((response) => response.json())
                    .then((result) => result.data.updateTask)
            ).pipe(
                mergeMap((task) => [updateTaskSuccess(task.id)])
            )
        )
    )

const deleteTaskEpic = (action$) =>
    action$.pipe(
        ofType(DELETE_TASK),
        mergeMap((action) =>
            from(
                fetch('https://localhost:7047/graphql', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json',
                    },
                    body: JSON.stringify({
                        query: DELETE_TASK_MUTATION,
                        variables: { id: action.payload },
                    })
                })
                    .then((response) => response.json())
                    .then((result) => result.data.deleteTask)
            ).pipe(
                mergeMap((task) => [deleteTaskSuccess(task.id)])
            )
        )
    )

export const rootEpic = combineEpics(fetchTasksEpic, createTaskEpic, updateTaskEpic, deleteTaskEpic);