function displayComments(comments) {
    var commentList = $('#commentList');
    commentList.empty();

    comments.forEach(function (comment) {
        var listItem = $('<li>').addClass('list-group-item d-flex justify-content-between align-items-center');

        var profileLink = $('<a>').attr('href', '/Profile/Index/' + comment.user.id).addClass('profile-link ml-3');
        var profileImage = $('<img>').attr('src', comment.user.profilePictureUrl || 'https://upload.wikimedia.org/wikipedia/commons/a/ac/Default_pfp.jpg').addClass('profile-image-sm rounded-circle me-2');
        var usernameSpan = $('<span>').text(comment.user.name);

        profileLink.append(profileImage, usernameSpan);

        var commentDiv = $('<div>').append(
            profileLink,
            $('<div>').append(
                $('<p>').text(comment.comment),
                $('<span>').text(formatDate(comment.commentDate))
            )
        );

        listItem.append(commentDiv);
        commentList.append(listItem);
    });
}

function formatDate(dateString) {
    var date = new Date(dateString);
    return date.toLocaleString();
}

$(document).on('click', '.view-more-comments-btn', function () {
    var postId = $(this).data('post-id');

    $.ajax({
        url: '/Home/Comments?postId=' + postId,
        type: 'GET',
        success: function (response) {
            displayComments(response.comments);
        }
    });
});

$(document).ready(function () {
    $('.btn-like').click(function (e) {
        e.preventDefault();

        var form = $(this).closest('form');
        var formData = form.serialize();

        $.ajax({
            url: form.attr('action'),
            type: 'POST',
            data: formData,
            success: function (response) {
                if (response.success) {
                    var likeButton = form.find('.btn-like');
                    var likeCountSpan = likeButton.find('.like-count');
                    var newLikeCount = response.newLikeCount;

                    likeCountSpan.text(newLikeCount);

                    if (likeButton.hasClass('btn-primary')) {
                        likeButton.removeClass('btn-primary').addClass('btn-secondary');
                    } else {
                        likeButton.removeClass('btn-secondary').addClass('btn-primary');
                    }
                }
            }
        });
    });

    $('.btn-comment').click(function (e) {
        e.preventDefault();

        var form = $(this).closest('form');

        var postId = form.find('input[name="postId"]').val();
        var commentText = form.find('input[name="comment"]').val();
        var commentInput = form.find('input[name="comment"]');

        $.ajax({
            url: form.attr('action'),
            type: 'POST',
            data: {
                postId: postId,
                comment: commentText
            },
            success: function (response) {
                var newComment = response.newComment;

                console.log(response);

                var profileImage = $('<img>').attr('src', newComment.profilePictureUrl || 'https://upload.wikimedia.org/wikipedia/commons/a/ac/Default_pfp.jpg').addClass('profile-image-sm rounded-circle me-2');

                var newCommentHtml = '<li class="list-group-item d-flex justify-content-between align-items-center">' +
                    '<div>' +
                    '<a href="/Profile/Index/' + response.userId + '" class="profile-link ml-3">' +
                    profileImage.prop('outerHTML') +
                    '<span>' + response.userName + '</span>' +
                    '</a>' +
                    '</div>' +
                    '<div>' +
                    '<span>' + response.newComment + '</span>' +
                    '</div>' +
                    '</li>';

                $('.list-group').append(newCommentHtml);

                commentInput.val('');
            }
        });
    });
});

$('#commentModal').on('show.bs.modal', function (event) {
    var button = $(event.relatedTarget);
    var postId = button.data('postid');
    var modal = $(this);
    modal.find('#postIdInput').val(postId);
});
