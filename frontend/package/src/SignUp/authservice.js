import axios from 'axios';
const USER_API = "http://localhost:8000/api/"
export const signup=async(user)=> {
return await axios.post(USER_API + "http://localhost:800/users/register",user);
}
// export const signin=async(user)=> {
//     // return await axios.post(USER_API + "users/login", user);
//     return await axios.post("http://localhost:5156/api/Account/Login", user);
// }

export const signin = async (userCredentials) => {
    try {
        const response = await axios.post(
            'http://localhost:5156/api/Account/Login',
            userCredentials,
            {
                headers: { 'Content-Type': 'application/json' },
                withCredentials: true, // Include credentials if needed
            }
        );
        console.log('API response:', response); // Log the API response to check if it's correct
        return response;
    } catch (error) {
        console.error('Signin error:', error); // Log any errors
        throw error;
    }
};


export const logout=async()=> {
return await axios.post(USER_API+"logout");
}
export const profile=async()=> {

return await axios.get(USER_API+"/user-profile");
}